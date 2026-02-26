using FluentValidation;
using VoterManagementSystem.Application.DTOs.Admin;

namespace VoterManagementSystem.Application.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(50)
                .WithMessage("Password cannot exceed 50 characters.");
        }
    }
}