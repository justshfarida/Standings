using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Standings.Application.DTOS.TokenDTOs;
using Standings.Application.Interfaces.ITokenHandler;
using Standings.Domain.Entities.AppDbContextEntity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Standings.Infrastructure.Implementations.TokenServices
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;
        readonly UserManager<User>  _userManager;
        public TokenHandler(IConfiguration configuration, UserManager<User> userManager)
        { 
            _configuration = configuration;
            _userManager = userManager;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<TokenDTO> CreateToken(User user)
        {
            TokenDTO tokenDTO = new TokenDTO();
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));//We need to convert this string to byte array so it could be encrypted by symmetric encryption algorithms.
            SigningCredentials signingCredentials=new(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name,user.UserName )
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            tokenDTO.ExpirationTime= DateTime.Now.AddMinutes(2);
            JwtSecurityToken securityToken = new JwtSecurityToken
                (
                    audience: _configuration["Token:Audience"],
                    issuer: _configuration["Token:Issuer"],
                    expires: tokenDTO.ExpirationTime,
                    signingCredentials: signingCredentials,
                    claims: claims
                );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            tokenDTO.AccessToken = tokenHandler.WriteToken(securityToken);
            tokenDTO.RefreshToken = CreateRefreshToken();
            return tokenDTO;

        }
    }
}
