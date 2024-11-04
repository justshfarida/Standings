using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.GroupDTOs;
using Standings.Application.DTOS.StudentDTOs;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Standings.Infrastructure.Implementations.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Group> _groupRepo;

        public GroupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _groupRepo = _unitOfWork.GetRepository<Group>();
        }

        public async Task<Response<GroupCreateDTO>> CreateGroup(GroupCreateDTO model)
        {
            var response = new Response<GroupCreateDTO> { Data = null, StatusCode = 400 };

            // Check if a group with the same name and year already exists
            var existingGroup = await _groupRepo.GetByCondition(g => g.Name == model.Name && g.Year == model.Year).FirstOrDefaultAsync();

            if (existingGroup != null)
            {
                response.Data = _mapper.Map<GroupCreateDTO>(existingGroup); // Return existing group details
                response.StatusCode = 409; // Conflict status code
                return response;
            }

            // If the group doesn't exist, proceed to create a new one
            var group = _mapper.Map<Group>(model);
            var result = await _groupRepo.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            if (result)
            {
                response.Data = _mapper.Map<GroupCreateDTO>(group);
                response.StatusCode = 201;
            }
            else
            {
                response.StatusCode = 500;
            }
            return response;
        }


        public async Task<Response<bool>> DeleteGroup(int id)
        {
            var response = new Response<bool> { Data = false, StatusCode = 500 };
            var result = await _groupRepo.RemoveByID(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                response.Data = true;
                response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = 404;
            }

            return response;
        }

        public async Task<Response<List<GroupGetDTO>>> GetAllGroups()
        {
            var response = new Response<List<GroupGetDTO>> { Data = null, StatusCode = 500 };
            var groups = await _groupRepo.GetAll().ToListAsync();
            if (groups != null)
            {
                var groupDTOs = _mapper.Map<List<GroupGetDTO>>(groups);
                response.Data = groupDTOs;
                response.StatusCode = 200;
            }
            return response;
        }

        public async Task<Response<GroupGetDTO>> GetGroupByYear(int year)
        {
            var response = new Response<GroupGetDTO> { Data = null, StatusCode = 500 };
            var group = await _groupRepo.GetByCondition(g => g.Year == year).FirstOrDefaultAsync();
            if (group != null)
            {
                response.Data = _mapper.Map<GroupGetDTO>(group);
                response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = 404;
            }
            return response;
        }

        public async Task<Response<IEnumerable<StudentGetDTO>>> GetTop5Students(int groupId)
        {
            var response = new Response<IEnumerable<StudentGetDTO>> { Data = null, StatusCode = 500 };

            var group = await _groupRepo.GetByIDAsync(groupId);
            if (group == null)
            {
                response.StatusCode = 404;
                return response;
            }

            var _studentRepo = _unitOfWork.GetRepository<Student>();

            // Retrieve students in the group and calculate their yearly averages by including Exam entity
            var studentsWithResults = await _studentRepo.GetByCondition(s => s.GroupId == groupId)
                .Select(s => new
                {
                    Student = s,
                    YearlyAverage = s.Results
                        .Where(r => r.Exam.ExamDate.Year == group.Year) // Filter results by the group's year
                        .Average(r => (r.Grade * r.Exam.Coefficient) / 20.0) // Calculate average using Exam.Coefficient
                })
                .OrderByDescending(sr => sr.YearlyAverage) // Order by calculated average
                .Take(5)
                .ToListAsync();

            // Map the top 5 students to StudentGetDTO and set the response data
            var topStudentsDTO = _mapper.Map<IEnumerable<StudentGetDTO>>(studentsWithResults.Select(sr => sr.Student));
            response.Data = topStudentsDTO;
            response.StatusCode = 200;

            return response;
        }



        public async Task<Response<double>> GetGroupAverage(int groupId)
        {
            var response = new Response<double> { Data = 0.0, StatusCode = 500 };
            var group = await _groupRepo.GetByIDAsync(groupId);
            if (group == null)
            {
                response.StatusCode = 404;
                return response;
            }

            var _studentRepo = _unitOfWork.GetRepository<Student>();
            var students = await _studentRepo.GetByCondition(s => s.GroupId == groupId).ToListAsync();

            // Calculate each student's average for the group's year using Exam.ExamDate and Exam.Coefficient
            var groupYearAverages = students
                .Select(s => s.Results
                    .Where(r => r.Exam.ExamDate.Year == group.Year)  // Filter by ExamDate year
                    .Average(r => (r.Grade * r.Exam.Coefficient) / 20.0))  // Calculate average using Exam.Coefficient
                .Where(avg => !double.IsNaN(avg))  // Exclude students without results for the year
                .ToList();

            response.Data = groupYearAverages.Any() ? groupYearAverages.Average() : 0.0;
            response.StatusCode = 200;
            return response;
        }

        public async Task<Response<bool>> UpdateGroup(GroupUpdateDTO model, int id)
        {
            var response = new Response<bool> { Data = false, StatusCode = 500 };
            var group = await _groupRepo.GetByIDAsync(id);
            if (group == null)
            {
                response.StatusCode = 404;
                return response;
            }

            _mapper.Map(model, group);
            var result = _groupRepo.Update(group);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                response.Data = true;
                response.StatusCode = 200;
            }
            return response;
        }
    }
}
