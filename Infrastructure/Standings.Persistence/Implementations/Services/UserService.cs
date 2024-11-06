using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.UserDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> AssignRoleToUser(string id, string[] roles)
        {
            var responseModel = new Response<bool> { Data = false, StatusCode = 400 };
            User user = await _userManager.FindByIdAsync(id);

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
            var responseModel = new Response<CreateUserResponseDTO> { Data = null, StatusCode = 400 };
            var responseDTO = new CreateUserResponseDTO { Message = "User not created", Succeeded = false };

            if (model != null)
            {
                var user = _mapper.Map<User>(model);
                user.Id = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    responseDTO.Message = string.Join("\n", result.Errors.Select(error => $"{error.Code} - {error.Description}"));
                    return responseModel;
                }

                responseDTO.Message = "User successfully created";
                responseDTO.Succeeded = true;
                responseModel.Data = responseDTO;
                responseModel.StatusCode = 201;

                // Assign "Student" role to the user
                await _userManager.AddToRoleAsync(user, "Student");

                // Step 1: Link UserId to Student based on email if match is found
                await LinkUserToStudentByEmail(user.Id, model.Email);
            }

            return responseModel;
        }

        public async Task<Response<bool>> DeleteUser(string id)
        {
            var responseModel = new Response<bool> { Data = false, StatusCode = 500 };
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                responseModel.StatusCode = 404; // User not found
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
            var responseModel = new Response<List<UserGetDTO>> { Data = null, StatusCode = 500 };

            // Load Users with related Student data
            var users = await _userManager.Users.Include(u => u.Student).ToListAsync();

            if (users != null)
            {
                var userDtos = _mapper.Map<List<UserGetDTO>>(users);
                responseModel.Data = userDtos;
                responseModel.StatusCode = 200;
            }

            return responseModel;
        }


        public async Task<Response<string[]>> GetRolesOfUser(string id)
        {
            var responseModel = new Response<string[]> { Data = null, StatusCode = 500 };
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
            var responseModel = new Response<UserGetDTO> { Data = null, StatusCode = 500 };

            // Use repository or context to load user with related Student entity
            var user = await _userManager.Users
                                         .Include(u => u.Student)  // Ensure Student is included
                                         .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                responseModel.StatusCode = 404;
                return responseModel;
            }

            // Map to UserGetDTO including StudentId if available
            var userGetDto = _mapper.Map<UserGetDTO>(user);
            responseModel.Data = userGetDto;
            responseModel.StatusCode = 200;

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
            var responseModel = new Response<bool> { Data = false, StatusCode = 500 };

            if (model != null)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    responseModel.StatusCode = 404;
                }
                else
                {
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

        // Helper Method: Link User to Student based on email
        private async Task LinkUserToStudentByEmail(string userId, string email)
        {
            var _studentRepo = _unitOfWork.GetRepository<Student>();

            // Find the student by email
            var student = await _studentRepo.GetByCondition(s => s.Email == email).FirstOrDefaultAsync();
            if (student != null)
            {
                student.UserId = userId; // Link UserId to Student
                await _unitOfWork.SaveChangesAsync(); // Save changes to update the student record
            }
        }
    }
}
