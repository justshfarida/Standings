using Standings.Application.DTOS.ExamDTOs;
using Standings.Application.Models.ResponseModels;

namespace Standings.Application.Interfaces.IServices
{
    public interface IExamService
    {
        Task<Response<List<ExamGetDTO>>> GetAllExams();
        Task<Response<ExamGetDTO>> GetExamById(int id);
        Task<Response<List<ExamGetDTO>>> ExamsByGroupId(int id);
        Task<Response<List<ExamGetDTO>>> ExamsByYear(int id);
        Task<Response<ExamCreateDTO>> CreateExam(ExamCreateDTO model);
        Task<Response<bool>> UpdateExam(ExamUpdateDTO model, int id);
        Task<Response<bool>> DeleteExam(int id);
    }
}
