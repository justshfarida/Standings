using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standings.Application.DTOS.StudentDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standings.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<List<StudentGetDTO>>>> GetAll()
        {
            var response = await _studentService.GetAllStudents();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<StudentGetDTO>>> GetById(int id)
        {
            var response = await _studentService.GetStudentById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("group/{groupId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<List<StudentGetDTO>>>> GetByGroupId(int groupId)
        {
            var response = await _studentService.StudentsByGroupId(groupId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("year/{year}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<List<StudentGetDTO>>>> GetByYear(int year)
        {
            var response = await _studentService.StudentsByYear(year);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<Response<StudentCreateDTO>>> Create(StudentCreateDTO model)
        {
            var response = await _studentService.CreateStudent(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = await _studentService.DeleteStudent(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<Response<bool>>> Update(int id, StudentUpdateDTO model)  
        {
            var response = await _studentService.UpdateStudent(model, id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
