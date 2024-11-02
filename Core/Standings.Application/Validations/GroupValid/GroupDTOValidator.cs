using FluentValidation;
using Standings.Application.DTOS.GroupDTOs;

namespace Standings.Application.Validations.GroupValid
{
    public class GroupDTOValidator : AbstractValidator<GroupCreateDTO>
    {
        public GroupDTOValidator()
        {
            // Create Validator for GroupCreateDTO
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Group Name can't be empty")
                .MaximumLength(100).WithMessage("Group Name must be less than 100 characters.");

            RuleFor(x => x.Year)
                .GreaterThan(0).WithMessage("Year must be a positive integer.")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future.");
        }

        // Validators for GroupGetDTO
        public class GroupGetDTOValidator : AbstractValidator<GroupGetDTO>
        {
            public GroupGetDTOValidator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("ID must be greater than 0.");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Group Name can't be empty")
                    .MaximumLength(100).WithMessage("Group Name must be less than 100 characters.");

                RuleFor(x => x.Year)
                    .GreaterThan(0).WithMessage("Year must be a positive integer.")
                    .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future.");
            }
        }

        // Validators for GroupUpdateDTO
        public class GroupUpdateDTOValidator : AbstractValidator<GroupUpdateDTO>
        {
            public GroupUpdateDTOValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Group Name can't be empty")
                    .MaximumLength(100).WithMessage("Group Name must be less than 100 characters.");

                RuleFor(x => x.Year)
                    .GreaterThan(0).WithMessage("Year must be a positive integer.")
                    .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future.");
            }
        }
    }
}
