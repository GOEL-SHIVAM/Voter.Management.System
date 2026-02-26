using VoterManagementSystem.Application.DTOs.Auth;

namespace VoterManagementSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> AdminLoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> VoterLoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> VoterRegisterAsync(VoterRegisterDto request);
    }
}