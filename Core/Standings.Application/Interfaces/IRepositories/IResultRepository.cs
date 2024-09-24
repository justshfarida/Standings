using Standings.Domain.Entities.AppDbContextEntity;
namespace Standings.Application.Interfaces.IRepositories
{
    public interface IResultRepository
    {
        Task<StudentExamResult> GetByStudentAndExamAsync(int studentId, int examId); // Student və Exam ID-lərlə axtarış
        Task<bool> AddAsync(StudentExamResult data);
        bool Remove(StudentExamResult data);
        bool Update(StudentExamResult data);
        IQueryable<StudentExamResult> GetByCondition(System.Linq.Expressions.Expression<Func<StudentExamResult, bool>> expression);
        Task<bool> RemoveByID(int examId, int studentId );
        IQueryable<StudentExamResult> GetAll();


    }
}
