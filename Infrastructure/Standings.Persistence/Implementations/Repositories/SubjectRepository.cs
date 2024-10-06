using Standings.Application.Interfaces.IRepositories;
using Standings.Persistence.Contexts;
using Standings.Domain.Entities.AppDbContextEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Repositories
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext context) : base(context)
        {
        }
    }
}
