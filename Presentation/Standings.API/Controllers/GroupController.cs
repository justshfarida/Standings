using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standings.Application.DTOS.GroupDTOs;
using Standings.Application.DTOS.StudentDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standings.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<List<GroupGetDTO>>>> GetAll()
        {
            var response = await _groupService.GetAllGroups();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{year}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<GroupGetDTO>>> GetByYear(int year)
        {
            var response = await _groupService.GetGroupByYear(year);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{groupId}/top5students")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<List<StudentGetDTO>>>> GetTop5Students(int groupId)
        {
            var response = await _groupService.GetTop5Students(groupId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{groupId}/average")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<double>>> GetGroupAverage(int groupId)
        {
            var response = await _groupService.GetGroupAverage(groupId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]
        public async Task<ActionResult<Response<GroupCreateDTO>>> Create(GroupCreateDTO model)
        {
            var response = await _groupService.CreateGroup(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = await _groupService.DeleteGroup(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]
        public async Task<ActionResult<Response<bool>>> Update(int id, GroupUpdateDTO model)
        {
            var response = await _groupService.UpdateGroup(model, id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
