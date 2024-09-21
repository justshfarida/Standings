using Standings.Domain.Entities.AppDbContextEntity;
using AutoMapper;
using Standings.Application.DTOS.ExamDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standings.Application.DTOS.UserDTOs;
using Standings.Application.DTOS.ResultDTOs;

namespace Standings.Application.Automappers
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            CreateMap<Exam, ExamGetDTO>().ReverseMap();
            CreateMap<Exam, ExamCreateDTO>().ReverseMap();
            CreateMap<Exam, ExamUpdateDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserGetDTO>().ReverseMap();
            CreateMap<StudentExamResult, ResultGetDTO>().ReverseMap();
            CreateMap<StudentExamResult, ResultUpdateDTO>().ReverseMap();
            CreateMap<StudentExamResult, ResultCreateDTO>().ReverseMap();

        }
    }
}
