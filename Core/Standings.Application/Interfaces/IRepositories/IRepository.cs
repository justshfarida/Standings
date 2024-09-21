using Standings.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;


namespace Standings.Application.Interfaces.IRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
        IQueryable<T> GetAll();
        Task<T> GetByIDAsync(int id);
        Task<bool> AddAsync(T data);
        bool Remove(T data);
        Task<bool> RemoveByID(int id);
        bool Update(T data);
        IQueryable<T> GetByCondition(System.Linq.Expressions.Expression<System.Func<T, bool>> expression);

    }
}
