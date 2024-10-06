using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standings.Application.DTOS.UserDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;

namespace Standings.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<Response<List<UserGetDTO>>>> GetAll()
        {
            var response = await _userService.GetAllUsers();
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<ActionResult<Response<CreateUserResponseDTO>>> Create(UserCreateDTO user)
        {
            var response = await _userService.CreateUser(user);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<bool>>> Delete(string user)
        {
            var response = await _userService.DeleteUser(user);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<bool>>> Update(UserUpdateDTO model)
        {
            var response = await _userService.UpdateUser(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("get-roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<ActionResult<Response<string>>> GetRolesOf(string userIdOrName)
        {
            var response = await _userService.GetRolesOfUser(userIdOrName);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("assign-roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> AssignRoleTo(string userId, string[] roles)
        {
            var response = await _userService.AssignRoleToUser(userId, roles);
            return StatusCode(response.StatusCode, response);

        }
    }
}
