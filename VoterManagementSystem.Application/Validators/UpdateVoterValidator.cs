using FluentValidation;
using VoterManagementSystem.Application.DTOs.Voter;

namespace VoterManagementSystem.Application.Validators
{
    public class UpdateVoterValidator : AbstractValidator<UpdateVoterDto>
    {
        public UpdateVoterValidator()
        {
            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name)
                    .MinimumLength(2)
                    .WithMessage("Name must be at least 2 characters long.")
                    .MaximumLength(100)
                    .WithMessage("Name cannot exceed 100 characters.")
                    .Matches(@"^[a-zA-Z\s]+$")
                    .WithMessage("Name can only contain letters and spaces.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters long.")
                    .MaximumLength(50)
                    .WithMessage("Password cannot exceed 50 characters.");
            });
        }
    }
}