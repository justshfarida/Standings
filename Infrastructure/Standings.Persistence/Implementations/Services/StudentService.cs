using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.StudentDTOs;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standings.Infrastructure.Implementations.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepo;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _studentRepo = _unitOfWork.GetRepository<Student>();
        }

        public async Task<Response<StudentCreateDTO>> CreateStudent(StudentCreateDTO studentCreateDTO)
        {
            Response<StudentCreateDTO> responseModel = new Response<StudentCreateDTO>() { Data = null, StatusCode = 400 };

            if (studentCreateDTO != null)
            {
                //qrup var mi onu yoxla
                var groupRepo = _unitOfWork.GetRepository<Group>();
                var existingGroup=await groupRepo.GetByCondition(g => g.Name == studentCreateDTO.GroupName && g.Year == studentCreateDTO.GroupYear).FirstOrDefaultAsync();
                Group group;
                if (existingGroup != null)
                {
                    group=existingGroup;
                }
                else
                {
                    //  Ele bir qrup yoxdursa tezesini yarat
                    group = new Group
                    {
                        Name = studentCreateDTO.GroupName,
                        Year = studentCreateDTO.GroupYear
                    };
                    await groupRepo.AddAsync(group);
                    await _unitOfWork.SaveChangesAsync();  //qrupu saxla ki id si de yaransin
                }
                // Əllə mapləmə - DTO-dan entitiyə məlumatları köçürürük
                var data = new Student
                {
                    Id = studentCreateDTO.Id,
                    FirstName = studentCreateDTO.FirstName,
                    LastName = studentCreateDTO.LastName,
                    Email = studentCreateDTO.Email,
                    UserId = studentCreateDTO.UserId,
                    GroupId = group.Id  // Yaradılan və ya mövcud qrupun ID-si
                };
                await _studentRepo.AddAsync(data);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    responseModel.Data = studentCreateDTO;
                    responseModel.StatusCode = 201;
                }
                else
                {
                    responseModel.StatusCode = 500;
                }
            }

            return responseModel;
        }

        public async Task<Response<bool>> DeleteStudent(int id)
        {
            var responseModel = new Response<bool>() { Data = false, StatusCode = 400 };
            var student = await _studentRepo.GetByIDAsync(id);

            if (student != null)
            {
                _studentRepo.Remove(student);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    responseModel.Data = true;
                    responseModel.StatusCode = 200;
                }
                else
                {
                    responseModel.StatusCode = 500;
                }
            }

            return responseModel;
        }

        public async Task<Response<List<StudentGetDTO>>> GetAllStudents()
        {
            var responseModel = new Response<List<StudentGetDTO>>() { Data = null, StatusCode = 400 };
            var students = await _studentRepo.GetAll().Include(s => s.Group).ToListAsync();//Eager Loading

            if (students != null)
            {
                var data = _mapper.Map<List<StudentGetDTO>>(students);
                responseModel.Data = data;
                responseModel.StatusCode = 200;
            }

            return responseModel;
        }

        public async Task<Response<StudentGetDTO>> GetStudentById(int id)
        {
            var responseModel = new Response<StudentGetDTO>() { Data = null, StatusCode = 400 };
            var student = await _studentRepo.GetAll().Include(s=>s.Group).FirstOrDefaultAsync(s=>s.Id==id);//getByIdAsync islede bilmirik cunki FindAsync və ya FirstOrDefaultAsync kimi birbaşa bir entiti üzərində çalışır və əlaqəli entitiləri yükləmir.


            if (student != null)
            {
                var data = _mapper.Map<StudentGetDTO>(student);
                responseModel.Data = data;
                responseModel.StatusCode = 200;
            }

            return responseModel;
        }

        public async Task<Response<List<StudentGetDTO>>> StudentsByGroupId(int id)
        {
            var responseModel = new Response<List<StudentGetDTO>>() { Data = null, StatusCode = 400 };
            var students = _studentRepo.GetByCondition(s => s.GroupId == id);

            if (students != null)
            {
                var data = _mapper.Map<List<StudentGetDTO>>(students);
                responseModel.Data = data;
                responseModel.StatusCode = 200;
            }

            return responseModel;
        }

        public async Task<Response<List<StudentGetDTO>>> StudentsByYear(int year)
        {
            var responseModel = new Response<List<StudentGetDTO>>() { Data = null, StatusCode = 400 };

            var students = await _studentRepo.GetByCondition(s => s.Group.Year == year)
                                             .Include(s => s.Group)
                                             .ToListAsync();

            if (students != null)
            {
                var data = _mapper.Map<List<StudentGetDTO>>(students);
                responseModel.Data = data;
                responseModel.StatusCode = 200;
            }

            return responseModel;
        }
        public async Task<Response<StudentGetDTO>> GetTopStudentByGroupId(int groupId)
        {
            var responseModel = new Response<StudentGetDTO>() { Data = null, StatusCode = 400 };

            // Qrup üzrə tələbələri və onların nəticələrini tapırıq
            var students = await _studentRepo.GetByCondition(s => s.GroupId == groupId)
                                             .Include(s => s.Results) // Nəticələri də daxil edirik
                                             .ToListAsync();

            if (students != null && students.Any())
            {
                // Tələbələrdən ən yüksək nəticəsi olanı tapırıq
                var topStudent = students
                    .Select(student => new
                    {
                        Student = student,
                        TotalResult = student.Results.Sum(r => r.Grade) // Hər tələbənin ümumi nəticəsi
                    })
                    .OrderByDescending(s => s.TotalResult) // Nəticələrə görə azalan sırada sıralayırıq
                    .FirstOrDefault()?.Student;

                if (topStudent != null)
                {
                    // Məlumatı DTO formatına çeviririk
                    var data = _mapper.Map<StudentGetDTO>(topStudent);
                    responseModel.Data = data;
                    responseModel.StatusCode = 200;
                }
            }

            return responseModel;
        }


        public async Task<Response<bool>> UpdateStudent(StudentUpdateDTO studentUpdateDTO, int id)
        {
            var responseModel = new Response<bool>() { Data = false, StatusCode = 400 };
            var student = await _studentRepo.GetByIDAsync(id);

            if (student != null)
            {
                _mapper.Map(studentUpdateDTO, student);
                _studentRepo.Update(student);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    responseModel.Data = true;
                    responseModel.StatusCode = 200;
                }
                else
                {
                    responseModel.StatusCode = 500;
                }
            }

            return responseModel;
        }
    }
}
