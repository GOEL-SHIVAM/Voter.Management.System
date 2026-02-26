using FluentValidation;
using VoterManagementSystem.Application.DTOs.Auth;

namespace VoterManagementSystem.Application.Validators
{
    public class VoterRegisterValidator : AbstractValidator<VoterRegisterDto>
    {
        public VoterRegisterValidator()
        {
            RuleFor(x => x.Aadhar)
                .NotEmpty().WithMessage("Aadhar number is required.")
                .Length(12).WithMessage("Aadhar must be exactly 12 digits.")
                .Matches(@"^\d{12}$").WithMessage("Aadhar must contain only digits.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.")
                .LessThan(DateTime.Today).WithMessage("Birth date must be in the past.")
                .Must(BeAtLeast18YearsOld).WithMessage("Voter must be at least 18 years old.");
        }

        private bool BeAtLeast18YearsOld(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}