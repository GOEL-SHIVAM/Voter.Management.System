using VoterManagementSystem.Application.DTOs.Auth;
using VoterManagementSystem.Application.Interfaces;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Enums;
using VoterManagementSystem.Core.Exceptions;
using VoterManagementSystem.Core.Interfaces;

namespace VoterManagementSystem.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IJwtService jwtService) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<LoginResponseDto> AdminLoginAsync(LoginRequestDto request)
        {
            var admin = await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.Username == request.Username);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(admin.Username, admin.Role.ToString(), admin.AdminId.ToString());

            return new LoginResponseDto
            {
                Token = token,
                Username = admin.Username,
                Role = admin.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<LoginResponseDto> VoterLoginAsync(LoginRequestDto request)
        {
            var voter = await _unitOfWork.Voters.FirstOrDefaultAsync(v => v.Aadhar == request.Username);

            if (voter == null || !BCrypt.Net.BCrypt.Verify(request.Password, voter.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(voter.Aadhar, UserRole.Voter.ToString(), voter.VoterId.ToString());

            return new LoginResponseDto
            {
                Token = token,
                Username = voter.Aadhar,
                Role = UserRole.Voter.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<LoginResponseDto> VoterRegisterAsync(VoterRegisterDto request)
        {
            var exists = await _unitOfWork.Voters.ExistsAsync(v => v.Aadhar == request.Aadhar);
            if (exists)
            {
                throw new VoterAlreadyExistsException(request.Aadhar);
            }

            var age = DateTime.Today.Year - request.BirthDate.Year;
            if (request.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
            {
                throw new VoterUnderAgeException(request.BirthDate);
            }

            var voter = new Voter
            {
                Aadhar = request.Aadhar,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name,
                BirthDate = request.BirthDate
            };

            await _unitOfWork.Voters.AddAsync(voter);
            await _unitOfWork.SaveChangesAsync();

            var token = _jwtService.GenerateToken(voter.Aadhar, UserRole.Voter.ToString(), voter.VoterId.ToString());

            return new LoginResponseDto
            {
                Token = token,
                Username = voter.Aadhar,
                Role = UserRole.Voter.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}