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
using Standings.Application.DTOS.StudentDTOs;
using Standings.Application.DTOS.GroupDTOs;

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
            CreateMap<User, UserGetDTO>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student != null ? (int?)src.Student.Id : null))
            .ReverseMap();

            CreateMap<StudentExamResult, ResultGetDTO>()
            .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Name))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Exam.Subject.Name)) // Map SubjectName
            .ReverseMap();
            CreateMap<StudentExamResult, ResultUpdateDTO>().ReverseMap();
            CreateMap<StudentExamResult, ResultCreateDTO>().ReverseMap();
            CreateMap<Student, StudentGetDTO>().ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name)).ReverseMap();
            CreateMap<Student, StudentUpdateDTO>().ReverseMap();
            CreateMap<GroupCreateDTO, Group>().ReverseMap();
            CreateMap<GroupUpdateDTO, Group>().ReverseMap();
            CreateMap<GroupGetDTO, Group>().ReverseMap();
        }
    }
}
