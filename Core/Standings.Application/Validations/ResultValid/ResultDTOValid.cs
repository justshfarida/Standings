using FluentValidation;
using Standings.Application.DTOS.ResultDTOs;

public class ResultCreateDTOValidator : AbstractValidator<ResultCreateDTO>
{
    public ResultCreateDTOValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId can not be empty");

        RuleFor(x => x.ExamId).NotEmpty().WithMessage("ExamId can not be empty");

        RuleFor(x => x.Grade).InclusiveBetween(0, 20)
            .WithMessage("Grade must be between 0 and 20");
    }
}

public class ResultGetDTOValidator : AbstractValidator<ResultGetDTO>
{
    public ResultGetDTOValidator()
    {

        RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId can not be empty")
            .GreaterThan(0).WithMessage("Student Id must be greater than 0.");

        RuleFor(x => x.ExamName).NotEmpty().WithMessage("Exam Name can not be empty");

        RuleFor(x => x.Grade).InclusiveBetween(0, 20)
            .WithMessage("Grade must be between 0 and 20");
    }
}

public class ResultUpdateDTOValidator : AbstractValidator<ResultUpdateDTO>
{
    public ResultUpdateDTOValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId can not be empty")
           .GreaterThan(0).WithMessage("Student Id must be greater than 0.");

        RuleFor(x => x.ExamId).NotEmpty().WithMessage("ExamId can not be empty").GreaterThan(0).WithMessage("Exam Id must be greater than 0.");

        RuleFor(x => x.Grade).InclusiveBetween(0, 20)
            .WithMessage("Grade must be between 0 and 20");
    }
}
