using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Domain.Entities.Common;
using Standings.Persistence.Contexts;
using Standings.Persistence.Implementations.Repositories;

namespace Standings.Persistence.Implementations.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext DbContext;
        private Dictionary<Type, object> _repositories;
        public UnitOfWork(AppDbContext dbContext) 
        {
            DbContext = dbContext;
            _repositories = new();
        }

        public void Dispose()
        {
            DbContext.Dispose();

        }
        // Method to get a repository for a specific entity type
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }
            IRepository<TEntity> repository = new Repository<TEntity>(DbContext);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();

        }
    }
}
