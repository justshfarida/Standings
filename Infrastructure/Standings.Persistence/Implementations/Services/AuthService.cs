using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.TokenDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.ITokenHandler;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly ITokenHandler _tokenHandler;
        readonly IUserService _userService;
        readonly IConfiguration _configuration;
        public AuthService(UserManager<User> userManager, SignInManager<User> signManager, ITokenHandler tokenHandler, IUserService userService, IConfiguration configuration ) 
        {
            _userManager = userManager;
            _signInManager = signManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
            _configuration = configuration;
        }
        public async Task<Response<TokenDTO>> LoginAsync(string userNameorEmail, string password)
        {

            Response<TokenDTO> responseModel = new()
            { Data = null, StatusCode = 400 };
            var user = await _userManager.FindByNameAsync(userNameorEmail);
            if (user == null)
            {
                responseModel.StatusCode = 404;
                return responseModel;
            }
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);//bir nece defe sehv giris bas vererse, hesabi locklamamaq ucun bu parametri false edirik
            if (result.Succeeded)
            {
                TokenDTO tokenDTO = await _tokenHandler.CreateToken(user);
                // Dəyəri string olaraq alıb sonra çevirmə edirik
                var minsString = _configuration["Token:RefreshTokenExpirationInMinutes"];
                var mins = Convert.ToDouble(minsString);
                await _userService.UpdateRefreshToken(tokenDTO.RefreshToken, user, tokenDTO.ExpirationTime.AddMinutes(mins));
                responseModel.Data = tokenDTO;
                responseModel.StatusCode = 200;
            }
            else
            {
                responseModel.StatusCode = 401;
            }
            return responseModel;
        }

        public async Task<Response<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken)
        {
            Response<TokenDTO> responseModel = new() { StatusCode = 400, Data = null };
            User user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user == null)
            {
                responseModel.StatusCode = 404;
                return responseModel;
            }
            if (user.RefreshTokenEndTime > DateTime.UtcNow)
            {
                TokenDTO token = await _tokenHandler.CreateToken(user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.ExpirationTime);
                responseModel.Data = token;
                responseModel.StatusCode = 200;
            }
            else
            {
                responseModel.StatusCode = 401;
            }
            return responseModel;
        }

        public async Task<Response<bool>> LogOut(string userNameorEmail)
        {
            Response<bool> responseModel = new() { Data = false, StatusCode = 400 };
            User user = await _userManager.FindByEmailAsync(userNameorEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userNameorEmail);
            }
            if (user == null)
            {
                responseModel.StatusCode = 404;
                return responseModel;
            }
            user.RefreshToken = null;
            user.RefreshTokenEndTime = null;//refresh tokeni silmek lazimdi
            var result = await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();//bizde yoxdu
            if (result.Succeeded)
            {
                responseModel.Data = true;
                responseModel.StatusCode = 200;
            }
            return responseModel;
        }

        public Task<Response<bool>> ResetPassword(string email, string curPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
