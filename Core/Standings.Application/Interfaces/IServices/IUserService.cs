using Standings.Application.DTOS.UserDTOs;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<Response<List<UserGetDTO>>> GetAllUsers();
        Task<Response<UserGetDTO>> GetUserById(string id);
        Task<Response<CreateUserResponseDTO>> CreateUser(UserCreateDTO model);
        Task<Response<bool>> DeleteUser(string id);
        Task<Response<bool>> UpdateUser(UserUpdateDTO model);
        Task<Response<bool>> AssignRoleToUser(string id, string[] roles);
        Task<Response<string[]>> GetRolesOfUser(string id);
        Task UpdateRefreshToken(string refreshToken, User user, DateTime accesTokenDate);
    }
}
    