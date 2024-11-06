using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.ResultDTOs;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;

namespace Standings.Persistence.Implementations.Services
{
    public class ResultService : IResultService
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IResultRepository _resultRepo;
        public ResultService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _resultRepo=_unitOfWork.Results;
        }

        public async Task<Response<ResultCreateDTO>> CreateResult(ResultCreateDTO model)
        {
            Response<ResultCreateDTO> response = new Response<ResultCreateDTO>() { Data = null, StatusCode = 400 };
            if (model != null)
            { 
                var result=_mapper.Map<StudentExamResult>(model);
                var added = await _resultRepo.AddAsync(result);
                if (added==true)
                {
                    response.Data = model;
                    response.StatusCode = 201;
                }
                else
                {
                    response.StatusCode = 500; 
                }
            }
            return response;
        }

        public async Task<Response<List<ResultGetDTO>>> GetAllResults()
        {
            Response<List<ResultGetDTO>> response = new Response<List<ResultGetDTO>>() { Data = null, StatusCode = 400 };
            var results = await _resultRepo.GetAll()
                                   .Include(r => r.Exam)  // Ensure Exam data is loaded
                                   .ToListAsync();
            if (results!=null)
            {
                var resultDTOs=_mapper.Map<List<ResultGetDTO>>(results);
                response.Data = resultDTOs;
                response.StatusCode = 200;
            }
            return response;
        }

        public async Task<Response<List<ResultGetDTO>>> GetResultsByExamId(int examId)
        {
            var response = new Response<List<ResultGetDTO>> { Data = null, StatusCode = 400 };

            if (examId <= 0)
            {
                response.StatusCode = 400;
                return response;
            }

            // Load results with Exam data to access ExamName
            var results = await _resultRepo.GetByCondition(r => r.ExamId == examId)
                                           .Include(r => r.Exam)  // Ensure Exam is included
                                           .ToListAsync();

            if (results == null || !results.Any())
            {
                response.StatusCode = 404; 
                return response;
            }

            // Map results to ResultGetDTO using AutoMapper
            var resultDTOs = _mapper.Map<List<ResultGetDTO>>(results);
            response.Data = resultDTOs;
            response.StatusCode = 200; // OK

            return response;
        }

        public async Task<Response<ResultUpdateDTO>> UpdateResult(ResultUpdateDTO model)
        {
            var response = new Response<ResultUpdateDTO> { Data = null, StatusCode = 400 };

            if (model == null)
            {
                response.StatusCode = 400; // Bad Request
                return response;
            }

            var result = await _resultRepo.GetByIDAsync(model.Id);

            if (result == null)
            {
                response.StatusCode = 404; // Not Found
                return response;
            }

            _mapper.Map(model, result);
            var isUpdated = _resultRepo.Update(result);

            if (isUpdated)
            {
                response.Data = model;
                response.StatusCode = 200; // OK
            }
            else
            {
                response.StatusCode = 500; // Internal Server Error
            }

            return response;
        }


        public async Task<Response<List<ResultGetDTO>>> GetResultsByStudentId(int studentId)
        {
            var response = new Response<List<ResultGetDTO>> { Data = null, StatusCode = 400 };

            if (studentId==null)
            {
                return response;
            }
                var results = await _resultRepo.GetByCondition(r => r.StudentId == studentId).ToListAsync();

                if (results == null || !results.Any())
                {
                    response.StatusCode = 404; // Not Found
                    return response;
                }

                var resultDTOs = _mapper.Map<List<ResultGetDTO>>(results);
                response.Data = resultDTOs;
                response.StatusCode = 200; // OK
            
            return response;
        }
        public async Task<Response<ResultGetDTO>> GetResultById(int resultId)
        {
            var response = new Response<ResultGetDTO> { Data = null, StatusCode = 400 };
            if(resultId<=0)
            {
                response.StatusCode = 400;
                return response;
            }

            // Load the result with Exam data to access ExamName
            var result = await _resultRepo.GetByCondition(r => r.Id == resultId)
                                          .Include(r => r.Exam)  // Ensure Exam is included
                                          .FirstOrDefaultAsync();

            if (result == null)
            {
                response.StatusCode = 404;
                return response;
            }

            // Map result to ResultGetDTO using AutoMapper
            var resultDTO = _mapper.Map<ResultGetDTO>(result);
            response.Data = resultDTO;
            response.StatusCode = 200; 

            return response;
        }

        public async Task<Response<bool>> DeleteResult(int resultId)
        {
            var response = new Response<bool> { Data = false, StatusCode = 400 };

            if (resultId<= 0)
            {
                response.StatusCode = 400; // Bad Request
                return response;
            }

            var result = await _resultRepo.GetByIDAsync(resultId);

            if (result == null)
            {
                response.StatusCode = 404; // Not Found
              return response;
            }

            var isDeleted = _resultRepo.Remove(result);
            if (isDeleted)
            {
                await _unitOfWork.SaveChangesAsync();
                response.Data = true;
                response.StatusCode = 200; // OK
            }
            else
            {
                response.StatusCode = 500; // Internal Server Error
            }

            return response;
        }

    }
}
