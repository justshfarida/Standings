using Standings.Application.DTOS.GroupDTOs;
using Standings.Application.DTOS.StudentDTOs;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standings.Application.Interfaces.IServices
{
    public interface IGroupService
    {
        Task<Response<GroupCreateDTO>> CreateGroup(GroupCreateDTO model);
        Task<Response<bool>> DeleteGroup(int id);
        Task<Response<List<GroupGetDTO>>> GetAllGroups();
        Task<Response<GroupGetDTO>> GetGroupByYear(int year);
        Task<Response<IEnumerable<StudentGetDTO>>> GetTop5Students(int groupId);
        //Task<Response<double>> GetGroupAverage(int groupId);
        Task<Response<bool>> UpdateGroup(GroupUpdateDTO model, int id);
        Task<Response<bool>> AddSubjectToGroup(int groupId, int subjectId);

    }
}
