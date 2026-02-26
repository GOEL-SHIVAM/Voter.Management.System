using VoterManagementSystem.Application.DTOs.Admin;

namespace VoterManagementSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDto?> GetAdminByUsernameAsync(string username);
        Task<List<AdminDto>> GetAllAdminsAsync();
        Task AddAdminAsync(CreateAdminDto request);
        Task DeleteAdminAsync(string username);
        Task ChangePasswordAsync(string username, string newPassword);
    }
}