using FluentValidation;
using VoterManagementSystem.Application.DTOs.Voter;

namespace VoterManagementSystem.Application.Validators
{
    public class CreateVoterValidator : AbstractValidator<CreateVoterDto>
    {
        public CreateVoterValidator()
        {
            RuleFor(x => x.Aadhar)
                .NotEmpty()
                .WithMessage("Aadhar number is required.")
                .Length(12)
                .WithMessage("Aadhar must be exactly 12 digits.")
                .Matches(@"^\d{12}$")
                .WithMessage("Aadhar must contain only digits.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(50)
                .WithMessage("Password cannot exceed 50 characters.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(2)
                .WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage("Birth date is required.")
                .LessThan(DateTime.Today)
                .WithMessage("Birth date must be in the past.")
                .Must(BeAtLeast18YearsOld)
                .WithMessage("Voter must be at least 18 years old to register.");
        }

        private bool BeAtLeast18YearsOld(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }
    }
}