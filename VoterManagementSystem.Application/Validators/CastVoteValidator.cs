using FluentValidation;
using VoterManagementSystem.Application.DTOs.Election;

namespace VoterManagementSystem.Application.Validators
{
    public class CastVoteValidator : AbstractValidator<CastVoteDto>
    {
        public CastVoteValidator()
        {
            RuleFor(x => x.ElectionCode)
                .NotEmpty()
                .WithMessage("Election code is required.");

            RuleFor(x => x.PartyName)
                .NotEmpty()
                .WithMessage("Party name is required.");
        }
    }
}