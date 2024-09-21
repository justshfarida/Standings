using Standings.Application.Models.ResponseModels;
using Standings.Application.DTOS.TokenDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<Response<TokenDTO>> LoginAsync(string userNameorEmail, string password);
        Task<Response<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken);
        Task<Response<bool>> LogOut(string userNameorEmail);
        Task<Response<bool>> ResetPassword(string email, string curPassword, string newPassword);
    }
}
