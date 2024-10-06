using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standings.Application.DTOS.ResultDTOs;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;

namespace Standings.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultController : ControllerBase
    {
        readonly IResultService _resultService;
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
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Moderator, Student")]
        public async Task<ActionResult<Response<ResultGetDTO>>> GetById(int examId, int studentId)
        {
            var response = await _resultService.GetResultById(examId, studentId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
