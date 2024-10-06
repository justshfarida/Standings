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
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context)
        {
        }
    }
}
