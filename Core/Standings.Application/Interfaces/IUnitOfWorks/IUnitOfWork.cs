using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.Common;
namespace Standings.Application.Interfaces.IUnitOfWorks;
public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    IResultRepository Results { get; }
    Task<int> SaveChangesAsync();
}
