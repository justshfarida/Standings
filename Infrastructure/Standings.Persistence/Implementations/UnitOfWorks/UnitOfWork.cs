using Standings.Application.Interfaces.IRepositories;
using Standings.Persistence.Contexts;
using Standings.Persistence.Implementations.Repositories;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Domain.Entities.Common;
namespace Standings.Persistence.Implementations.UnitOfWorks;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }
        IRepository<TEntity> repository = new Repository<TEntity>(_dbContext);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public IResultRepository Results => new ResultRepository(_dbContext); // Assuming you create a constructor for ResultRepository that takes AppDbContext

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
