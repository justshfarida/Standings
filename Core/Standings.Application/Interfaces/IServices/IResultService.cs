using Standings.Application.DTOS.ResultDTOs;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.Interfaces.IServices
{
    public interface IResultService
    {
        Task<Response<ResultCreateDTO>> CreateResult(ResultCreateDTO model);
        Task<Response<ResultUpdateDTO>> UpdateResult(ResultUpdateDTO model);
        Task<Response<List<ResultGetDTO>>> GetAllResults();
        Task<Response<ResultGetDTO>> GetResultById(int examId, int studentId);
        Task<Response<List<ResultGetDTO>>> GetResultsByExamId(int examId);
        Task<Response<List<ResultGetDTO>>> GetResultsByStudentId(int studentId);
        Task<Response<bool>> DeleteResult(int examId, int studentId);
        }

}
