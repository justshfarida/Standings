using Standings.Domain.Entities.AppDbContextEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.Interfaces.IRepositories
{
    public interface IGroupRepository:IRepository<Group>
    {
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<Group> GetByYearAsync(int year);
        Task<IEnumerable<Student>> GetTop5StudentsAsync(int groupId);
        Task<double> GetGroupAverageAsync(int groupId);
    }
}
