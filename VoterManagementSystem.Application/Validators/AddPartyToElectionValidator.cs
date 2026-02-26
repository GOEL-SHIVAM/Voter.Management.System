using FluentValidation;
using VoterManagementSystem.Application.DTOs.Party;

namespace VoterManagementSystem.Application.Validators
{
    public class AddPartyToElectionValidator : AbstractValidator<AddPartyToElectionDto>
    {
        public AddPartyToElectionValidator()
        {
            RuleFor(x => x.PartyName)
                .NotEmpty()
                .WithMessage("Party name is required.")
                .MinimumLength(2)
                .WithMessage("Party name must be at least 2 characters long.")
                .MaximumLength(100)
                .WithMessage("Party name cannot exceed 100 characters.");
        }
    }
}