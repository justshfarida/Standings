using Microsoft.EntityFrameworkCore;
using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Repositories
{
    public class ResultRepository:IResultRepository
    {
        private AppDbContext _dbContext { get; set; }

        public ResultRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddAsync(StudentExamResult data)
        {
            await _dbContext.Results.AddAsync(data);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IQueryable<StudentExamResult> GetByCondition(Expression<Func<StudentExamResult, bool>> expression)
        {
            return _dbContext.Results.Where(expression);
        }

        public async Task<StudentExamResult> GetByStudentAndExamAsync(int studentId, int examId)
        {
            return await _dbContext.Results.FirstOrDefaultAsync(res => res.StudentId == studentId && res.ExamId == examId);
        }

        public bool Remove(StudentExamResult data)
        {
            _dbContext.Results.Remove(data);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> RemoveByID(int examId, int studentId)
        {
            var result = await GetByStudentAndExamAsync(studentId, examId);
            if (result != null)
            {
                return Remove(result);
            }
            return false;
        }

        public bool Update(StudentExamResult data)
        {
            _dbContext.Results.Update(data);
            return _dbContext.SaveChanges() > 0;
        }

        public IQueryable<StudentExamResult> GetAll()
        {
            return _dbContext.Results.AsQueryable();
        }
    }
}
