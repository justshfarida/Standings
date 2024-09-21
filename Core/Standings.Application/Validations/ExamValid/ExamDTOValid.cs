using FluentValidation;
using Standings.Application.DTOS.ExamDTOs;

namespace Standings.Application.Validations.ExamValid
{
    public class ExamDTOValidator : AbstractValidator<ExamCreateDTO>
    {
        public ExamDTOValidator()
        {
            // Create Validator
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Exam Name can't be empty")
                .MaximumLength(100).WithMessage("Exam Name must be less than 100 characters.");

            RuleFor(x => x.ExamDate)
                .NotEmpty().WithMessage("Exam Date can't be empty")
                .GreaterThan(DateTime.Now).WithMessage("Exam Date must be in the future.");

            RuleFor(x => x.Coefficient)
                .GreaterThan(0).WithMessage("Coefficient must be greater than 0.");

            RuleFor(x => x.SubjectId)
                .GreaterThan(0).WithMessage("Subject ID must be greater than 0.");
        }

        // validators for ExamGetDTO and ExamUpdateDTO
        public class ExamGetDTOValidator : AbstractValidator<ExamGetDTO>
        {
            public ExamGetDTOValidator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("ID must be greater than 0.");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Exam Name can't be empty")
                    .MaximumLength(100).WithMessage("Exam Name must be less than 100 characters.");

                RuleFor(x => x.ExamDate)
                    .NotEmpty().WithMessage("Exam Date can't be empty");

                RuleFor(x => x.Coefficient)
                    .GreaterThan(0).WithMessage("Coefficient must be greater than 0.");

                RuleFor(x => x.SubjectId)
                    .GreaterThan(0).WithMessage("Subject ID must be greater than 0.");
            }
        }

        public class ExamUpdateDTOValidator : AbstractValidator<ExamUpdateDTO>
        {
            public ExamUpdateDTOValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Exam Name can't be empty")
                    .MaximumLength(100).WithMessage("Exam Name must be less than 100 characters.");

                RuleFor(x => x.ExamDate)
                    .NotEmpty().WithMessage("Exam Date can't be empty")
                    .GreaterThan(DateTime.Now).WithMessage("Exam Date must be in the future.");

                RuleFor(x => x.Coefficient)
                    .GreaterThan(0).WithMessage("Coefficient must be greater than 0.");

                RuleFor(x => x.SubjectId)
                    .GreaterThan(0).WithMessage("Subject ID must be greater than 0.");
            }
        }
    }
}
