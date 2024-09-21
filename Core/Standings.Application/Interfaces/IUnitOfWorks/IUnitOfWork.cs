using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.Common;

namespace Standings.Application.Interfaces.IUnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        //ISchoolRepository Schools {  get; }
        //IStudentRepository Students { get; }
        //void Commit();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        Task<int> SaveChangesAsync();

    }
}
