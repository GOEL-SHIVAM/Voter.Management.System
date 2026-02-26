using FluentValidation;
using VoterManagementSystem.Application.DTOs.Admin;

namespace VoterManagementSystem.Application.Validators
{
    public class CreateAdminValidator : AbstractValidator<CreateAdminDto>
    {
        public CreateAdminValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("Username can only contain letters, numbers, and underscores.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(50)
                .WithMessage("Password cannot exceed 50 characters.");
        }
    }
}