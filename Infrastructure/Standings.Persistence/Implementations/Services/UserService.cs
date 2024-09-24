using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Standings.Application.DTOS.UserDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;
        public UserService(UserManager<User> userManager, IMapper mapper ) {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Response<bool>> AssignRoleToUser(string id, string[] roles)
        {
            User user = await _userManager.FindByIdAsync(id);

            Response<bool> responseModel = new Response<bool>
            {
                Data = false,
                StatusCode = 400
            };
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                IdentityResult result = await _userManager.AddToRolesAsync(user, roles);
                if (result.Succeeded)
                {
                    responseModel.Data = true;
                    responseModel.StatusCode = 200;
                }
            }
            return responseModel;
        }

        public async Task<Response<CreateUserResponseDTO>> CreateUser(UserCreateDTO model)
        {
            Response<CreateUserResponseDTO> responseModel = new Response<CreateUserResponseDTO>()
            {
                Data = null,
                StatusCode = 400
            };

            CreateUserResponseDTO responseDTO = new CreateUserResponseDTO()
            {
                Message = "User not created",
                Succeeded = false
            };
            if (model != null)
            {
                var user = _mapper.Map<User>(model);//Mapper lazim burda yoxsa elle maplemeliyik?
                user.Id = Guid.NewGuid().ToString(); // Ensure Id is set
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    responseDTO.Message = string.Join("\n", result.Errors.Select(error => $"{error.Code}-{error.Description}"));
                    return responseModel;
                }

                responseDTO.Message = "User Successfully created";
                responseDTO.Succeeded = true;


                responseModel.Data = responseDTO;
                responseModel.StatusCode = 201;

            }
            User _user = await _userManager.FindByNameAsync(model.UserName);//used just to retrieve the user with one of the three ways
            if (_user == null)
            {
                _user = await _userManager.FindByEmailAsync(model.Email);
                if (_user == null)
                {
                    _user = await _userManager.FindByIdAsync(_user.Id);

                }

            }
            if (_user != null)
                await _userManager.AddToRoleAsync(_user, "Student");

            return responseModel;
        }

        public async Task<Response<bool>> DeleteUser(string id)
        {
            Response<bool> responseModel = new Response<bool> { Data = false, StatusCode = 500 };
            var user=await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                responseModel.StatusCode = 404;//user not found
            }
            else
            {
                 var result = await _userManager.DeleteAsync(user); 
                if (result.Succeeded)
                {
                    responseModel.Data = true;
                    responseModel.StatusCode = 200;
                }
            }
            return responseModel;  
        }

        public async Task<Response<List<UserGetDTO>>> GetAllUsers()
        {
            Response<List<UserGetDTO>> responseModel = new Response<List<UserGetDTO>> { Data = null, StatusCode=500 };
            var users=_userManager.Users.ToList();
            if(users != null)
            {
                List<UserGetDTO> userDtos = _mapper.Map<List<UserGetDTO>>(users);
                responseModel.Data = userDtos;
                responseModel.StatusCode=200;
            }
            return responseModel;
        }

        public async Task<Response<string[]>> GetRolesOfUser(string id)
        {
            Response<string[]> responseModel = new Response<string[]>() { Data = null, StatusCode = 500 };
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                responseModel.StatusCode = 404;
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                responseModel.Data = userRoles.ToArray();
                responseModel.StatusCode = 200;
            }
            return responseModel;
        }

        public async Task<Response<UserGetDTO>> GetUserById(string id)
        {
            Response<UserGetDTO> responseModel = new Response<UserGetDTO> {Data=null, StatusCode=500};  
            var user=await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                responseModel.StatusCode = 404;
            }
            else
            {
                var userGetDto=_mapper.Map<UserGetDTO>(user);
                responseModel.Data = userGetDto;
                responseModel.StatusCode=200;
            }
            return responseModel;
        }

        public async Task UpdateRefreshToken(string refreshToken, User user, DateTime accesTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndTime = accesTokenDate;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<Response<bool>> UpdateUser(UserUpdateDTO model)
        {
            var responseModel = new Response<bool>
            {
                Data = false,
                StatusCode = 500
            };

            if (model != null)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    responseModel.StatusCode = 404;
                }
                else
                {
                    // Map updated fields
                    user = _mapper.Map<User>(model);

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        responseModel.Data = true;
                        responseModel.StatusCode = 200;
                    }
                }
            }
            return responseModel;
        }
    }
}
