using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.ExamDTOs;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;

namespace Standings.Infrastructure.Implementations.Services
{
    public class ExamService : IExamService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Exam> _examRepo;

        public ExamService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _examRepo = _unitOfWork.GetRepository<Exam>();
        }

        public async Task<Response<ExamCreateDTO>> CreateExam(ExamCreateDTO model)
        {
            var response = new Response<ExamCreateDTO> { Data = null, StatusCode = 500 };
            // Mövcud subjecti yoxlamaq
            var _subjectRepo = _unitOfWork.GetRepository<Subject>();
            var existingSubject = await _subjectRepo.GetByCondition(s => s.Name == model.SubjectName).FirstOrDefaultAsync();

            // Əgər subject mövcud deyilsə, yeni subject yarat
            if (existingSubject == null)
            {
                var newSubject = new Subject
                {
                    Name = model.SubjectName
                };

                await _subjectRepo.AddAsync(newSubject);
                await _unitOfWork.SaveChangesAsync();

                existingSubject = newSubject; // Yaradılan subjecti set et
            }
            var exam = _mapper.Map<Exam>(model);
            exam.SubjectId = existingSubject.Id;
            var result = await _examRepo.AddAsync(exam);
            await _unitOfWork.SaveChangesAsync();

            if (result)
            {
                response.Data = _mapper.Map<ExamCreateDTO>(exam);
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

        public async Task<Response<List<ExamGetDTO>>> ExamsByGroupId(int id)
        {
            var response = new Response<List<ExamGetDTO>> { Data = null, StatusCode = 500 };
            var exams = _examRepo.GetByCondition(x => x.SubjectId == id);
            if (exams != null)
            {
                var examDTOs = _mapper.Map<List<ExamGetDTO>>(exams);
                response.Data = examDTOs;
                response.StatusCode = 200;
            }

            return response;
        }

        public async Task<Response<List<ExamGetDTO>>> ExamsByYear(int year)
        {
            var response = new Response<List<ExamGetDTO>> { Data = null, StatusCode = 500 };
            var exams = _examRepo.GetByCondition(x => x.ExamDate.Year == year);
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
