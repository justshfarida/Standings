using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standings.Application.Models.ResponseModels;
using Standings.Application.DTOS.StudentDTOs;
namespace Standings.Application.Interfaces.IServices
{
    public interface IStudentService
    {
        Task<Response<List<StudentGetDTO>>> GetAllStudents();
        Task<Response<StudentGetDTO>> GetStudentById(int id);
        Task<Response<List<StudentGetDTO>>> StudentsByGroupId(int id);
        Task<Response<List<StudentGetDTO>>> StudentsByYear(int id);
        Task<Response<StudentCreateDTO>> CreateStudent(StudentCreateDTO studentCreateDTO);
        Task<Response<bool>> UpdateStudent(StudentUpdateDTO studentUpdateDTO, int id);
        Task<Response<bool>> DeleteStudent(int id);
    }
}
