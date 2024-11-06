using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.ExamDTOs;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Persistence.Contexts;

namespace Standings.Infrastructure.Implementations.Services
{
    public class ExamService : IExamService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<Group> _groupRepo;
        private readonly AppDbContext _context;


        public ExamService(IMapper mapper, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _examRepo = _unitOfWork.GetRepository<Exam>();
            _context = context; // Direct access to DbContext
        }

        public async Task<Response<ExamCreateDTO>> CreateExam(ExamCreateDTO model)
        {
            var response = new Response<ExamCreateDTO> { Data = null, StatusCode = 500 };
            var _subjectRepo = _unitOfWork.GetRepository<Subject>();

            // Step 1: Check if the subject exists
            var existingSubject = await _subjectRepo.GetByCondition(s => s.Name == model.SubjectName).FirstOrDefaultAsync();

            // Step 2: If the subject doesn't exist, create it
            if (existingSubject == null)
            {
                var newSubject = new Subject
                {
                    Name = model.SubjectName
                };

                try
                {
                    await _subjectRepo.AddAsync(newSubject);
                    await _unitOfWork.SaveChangesAsync();
                    existingSubject = newSubject;
                }
                catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    // Retrieve the existing subject if unique constraint violation occurs
                    existingSubject = await _subjectRepo.GetByCondition(s => s.Name == model.SubjectName).FirstOrDefaultAsync();
                }
            }

            // Step 3: Check if an exam with the same name and subject already exists
            var existingExam = await _examRepo.GetByCondition(e => e.Name == model.Name && e.SubjectId == existingSubject.Id).FirstOrDefaultAsync();
            if (existingExam != null)
            {
                response.StatusCode = 409; // Conflict
                return response;
            }

            // Step 4: Create the new exam
            var exam = new Exam
            {
                Name = model.Name,
                ExamDate = model.ExamDate,
                Coefficient = model.Coefficient,
                SubjectId = existingSubject.Id
            };

            var result = await _examRepo.AddAsync(exam);
            await _unitOfWork.SaveChangesAsync();

            if (result)
            {
                response.Data = new ExamCreateDTO
                {
                    Name = exam.Name,
                    ExamDate = exam.ExamDate,
                    Coefficient = exam.Coefficient,
                    SubjectName = existingSubject.Name
                };
                response.StatusCode = 201;
            }
            return response;
        }


        public async Task<Response<bool>> DeleteExam(int id)
        {
            var response = new Response<bool> { Data = false, StatusCode = 500 };
            var result = await _examRepo.RemoveByID(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                response.Data = true;
                response.StatusCode = 200;
            }

            return response;
        }

        public async Task<Response<List<ExamGetDTO>>> ExamsByGroupId(int groupId)
        {
            var response = new Response<List<ExamGetDTO>> { Data = null, StatusCode = 500 };

            // Check if the group exists
            var group = await _unitOfWork.GetRepository<Group>().GetByIDAsync(groupId);
            if (group == null)
            {
                response.StatusCode = 404;
                return response;
            }

            // Use _context to access GroupSubjects directly
            var subjectIds = await _context.GroupSubjects
                                           .Where(gs => gs.GroupId == groupId)
                                           .Select(gs => gs.SubjectId)
                                           .ToListAsync();

            if (!subjectIds.Any())
            {
                response.StatusCode = 404;
                return response;
            }

            // Retrieve exams associated with the subjects found
            var exams = await _examRepo.GetByCondition(e => subjectIds.Contains(e.SubjectId))
                                       .ToListAsync();

            // Map exams to DTOs and set the response
            var examDTOs = _mapper.Map<List<ExamGetDTO>>(exams);
            response.Data = examDTOs;
            response.StatusCode = 200;

            return response;
        }

        public async Task<Response<List<ExamGetDTO>>> ExamsByYear(int year)
        {
            var response = new Response<List<ExamGetDTO>> { Data = null, StatusCode = 500 };
            var exams = _examRepo.GetByCondition(x => x.ExamDate.Year == year).Include(e => e.Subject); // if Subject is related
            if (exams != null)
            {
                var examDTOs = _mapper.Map<List<ExamGetDTO>>(exams);
                response.Data = examDTOs;
                response.StatusCode = 200;
            }
            return response;
        }

        public async Task<Response<List<ExamGetDTO>>> GetAllExams()
        {
            var response = new Response<List<ExamGetDTO>> { Data = null, StatusCode = 500 };
            var exams = await _examRepo.GetAll().ToListAsync();
            if (exams != null)
            {
                var examDTOs = _mapper.Map<List<ExamGetDTO>>(exams);
                response.Data = examDTOs;
                response.StatusCode = 200;
            }
            return response;
        }

        public async Task<Response<ExamGetDTO>> GetExamById(int id)
        {
            var response = new Response<ExamGetDTO> { Data = null, StatusCode = 500 };
            var exam = await _examRepo.GetByIDAsync(id);
            if (exam != null)
            {
                var examDTO = _mapper.Map<ExamGetDTO>(exam);
                response.Data = examDTO;
                response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = 404;
            }

            return response;
        }

        public async Task<Response<bool>> UpdateExam(ExamUpdateDTO model, int id)
        {
            var response = new Response<bool> { Data = false, StatusCode = 500 };
            var exam = await _examRepo.GetByIDAsync(id);
            if (exam == null)
            {
                response.StatusCode = 404;
                return response;
            }

            _mapper.Map(model, exam);
            var result = _examRepo.Update(exam);
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
