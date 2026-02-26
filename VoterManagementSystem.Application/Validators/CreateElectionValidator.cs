using FluentValidation;
using VoterManagementSystem.Application.DTOs.Election;

namespace VoterManagementSystem.Application.Validators
{
    public class CreateElectionValidator : AbstractValidator<CreateElectionDto>
    {
        public CreateElectionValidator()
        {
            RuleFor(x => x.ElectionCode)
                .NotEmpty()
                .WithMessage("Election code is required.")
                .MinimumLength(3)
                .WithMessage("Election code must be at least 3 characters long.")
                .MaximumLength(50)
                .WithMessage("Election code cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9-_]+$")
                .WithMessage("Election code can only contain letters, numbers, hyphens, and underscores.");
        }
    }
}