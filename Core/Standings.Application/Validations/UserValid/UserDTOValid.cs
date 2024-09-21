using FluentValidation;
using Standings.Application.DTOS.UserDTOs;
namespace Standings.Application.Validations.UserValid
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDTOValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(20).WithMessage("Username must be at most 20 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
    public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(20).WithMessage("Username must be at most 20 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
    public class UserGetDTOValidator : AbstractValidator<UserGetDTO>
    {
        public UserGetDTOValidator()
        {
            // Validate UserId
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID cannot be empty");

            // Validate UserName
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User name cannot be empty")
                .MaximumLength(50).WithMessage("User name must be less than 50 characters");

            // Validate Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("A valid email address is required");
        }

    }
}
