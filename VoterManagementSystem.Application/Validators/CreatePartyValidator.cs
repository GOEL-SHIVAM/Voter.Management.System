using FluentValidation;
using VoterManagementSystem.Application.DTOs.Party;

namespace VoterManagementSystem.Application.Validators
{
    public class CreatePartyValidator : AbstractValidator<CreatePartyDto>
    {
        public CreatePartyValidator()
        {
            RuleFor(x => x.PartyName)
                .NotEmpty()
                .WithMessage("Party name is required.")
                .MinimumLength(2)
                .WithMessage("Party name must be at least 2 characters long.")
                .MaximumLength(100)
                .WithMessage("Party name cannot exceed 100 characters.");

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