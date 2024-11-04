using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standings.Application.DTOS.ResultDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standings.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]
        public async Task<ActionResult<Response<ResultCreateDTO>>> Create(ResultCreateDTO model)
        {
            var response = await _resultService.CreateResult(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<List<ResultGetDTO>>>> GetAll()
        {
            var response = await _resultService.GetAllResults();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{examId}/{studentId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<ResultGetDTO>>> GetById(int examId, int studentId)
        {
            var response = await _resultService.GetResultById(examId, studentId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("exam/{examId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<List<ResultGetDTO>>>> GetByExamId(int examId)
        {
            var response = await _resultService.GetResultsByExamId(examId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("student/{studentId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<List<ResultGetDTO>>>> GetByStudentId(int studentId)
        {
            var response = await _resultService.GetResultsByStudentId(studentId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]
        public async Task<ActionResult<Response<ResultUpdateDTO>>> Update(ResultUpdateDTO model)
        {
            var response = await _resultService.UpdateResult(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{examId}/{studentId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator")]
        public async Task<ActionResult<Response<bool>>> Delete(int examId, int studentId)
        {
            var response = await _resultService.DeleteResult(examId, studentId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
