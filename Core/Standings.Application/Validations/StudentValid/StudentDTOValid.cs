using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Standings.Application.DTOS.StudentDTOs;

namespace Standings.Application.Validations.StudentValid
{

    public class StudentCreateDTOValidator : AbstractValidator<StudentCreateDTO>
    {
        public StudentCreateDTOValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id can not be empty").GreaterThan(0).WithMessage("ID must be greater than 0.");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty")
                .Length(2, 30).WithMessage("First name must be between 2 and 30 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty")
                .Length(2, 30).WithMessage("Last name must be between 2 and 30 characters");
        }
    }
    public class StudentGetDTOValidator : AbstractValidator<StudentGetDTO>
    {
        public StudentGetDTOValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Student ID must be greater than 0");
        }
    }
    public class StudentUpdateDTOValidator : AbstractValidator<StudentUpdateDTO>
    {
        public StudentUpdateDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty")
                .Length(2, 30).WithMessage("First name must be between 2 and 30 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty")
                .Length(2, 30).WithMessage("Last name must be between 2 and 30 characters");

            RuleFor(x => x.GroupId)
                .GreaterThan(0).WithMessage("Group ID must be greater than 0");
        }
    }



}
