using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Standings.Application.DTOS.ResultDTOs;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Persistence.Implementations.Services
{
    public class ResultService : IResultService
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<StudentExamResult> _resultRepo;
        public ResultService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _resultRepo=_unitOfWork.GetRepository<StudentExamResult>();
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
                    response.StatusCode = 500; // Internal Server Error
                }
            }
            return response;
        }

        public async Task<Response<List<ResultGetDTO>>> GetAllResults()
        {
            Response<List<ResultGetDTO>> response = new Response<List<ResultGetDTO>>() { Data = null, StatusCode = 400 };
            var results= _resultRepo.GetAll().ToList();
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

            if (examId==null)
            {
                response.StatusCode = 400; 
                return response;
            }
            var results = await _resultRepo.GetByCondition(r => r.ExamId == examId).ToListAsync();

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


        public async Task<Response<ResultGetDTO>> GetResultById(int id)
        {
            var response = new Response<ResultGetDTO> { Data=null, StatusCode=400};
            if (id==null)
            {
                response.StatusCode = 400;
                return response;
            }
            var result = await _resultRepo.GetByIDAsync(id);
            if (result == null)
            {
                response.StatusCode = 404; // Not Found
                return response;
            }
            throw new NotImplementedException();
        }

        public Task<Response<ResultUpdateDTO>> UpdateResult(ResultUpdateDTO model)
        {
            throw new NotImplementedException();
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
    }
}
