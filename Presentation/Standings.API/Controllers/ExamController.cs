using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standings.Application.DTOS.ExamDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standings.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<List<ExamGetDTO>>>> GetAll()
        {
            var response = await _examService.GetAllExams();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<ExamGetDTO>>> GetById(int id)
        {
            var response = await _examService.GetExamById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("group/{groupId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<List<ExamGetDTO>>>> GetByGroupId(int groupId)
        {
            var response = await _examService.ExamsByGroupId(groupId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("year/{year}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]

        public async Task<ActionResult<Response<List<ExamGetDTO>>>> GetByYear(int year)
        {
            var response = await _examService.ExamsByYear(year);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]

        public async Task<ActionResult<Response<ExamCreateDTO>>> Create(ExamCreateDTO model)
        {
            var response = await _examService.CreateExam(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]

        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = await _examService.DeleteExam(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]

        public async Task<ActionResult<Response<bool>>> Update(int id, ExamUpdateDTO model)
        {
            var response = await _examService.UpdateExam(model, id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
