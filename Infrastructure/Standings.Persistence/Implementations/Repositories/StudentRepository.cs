using Microsoft.EntityFrameworkCore;
using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Repositories
{
    public class StudentRepository :Repository<Student>,  IStudentRepository
    {
        public StudentRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
