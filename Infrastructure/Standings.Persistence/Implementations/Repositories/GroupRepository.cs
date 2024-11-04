using Microsoft.EntityFrameworkCore;
using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Persistence.Contexts;
using Standings.Persistence.Implementations.Repositories;
using System.Linq;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(AppDbContext context) : base(context)
    {
    }
}
