using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.Common;
using Standings.Persistence.Contexts;
using System.Linq.Expressions;

namespace Standings.Persistence.Implementations.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private AppDbContext _context { get; set; }
        public Repository(AppDbContext context) 
        { 
            _context = context;
        }
        public DbSet<T> Table =>_context.Set<T>();

        public async Task<bool> AddAsync(T data)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(data);
            return entityEntry.State == EntityState.Added;
        }

        public IQueryable<T> GetAll()
        {
            return Table.AsQueryable();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            return await Table.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Remove(T data)
        {
            EntityEntry<T> entityEntry = Table.Remove(data);
            return entityEntry.State==EntityState.Deleted;
        }

        public async Task<bool> RemoveByID(int id)
        {
           T data=await Table.FindAsync(id);
            if (data == null)
                return false; // Entity not found
            EntityEntry<T> entityEntry = Table.Remove(data);
            return entityEntry.State==EntityState.Deleted;
        }

        public bool Update(T data)
        {
            EntityEntry<T> entityEntry = Table.Update(data);
            return entityEntry.State == EntityState.Modified;
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }
    }
}
