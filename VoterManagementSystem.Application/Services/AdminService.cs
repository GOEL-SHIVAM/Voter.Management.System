using VoterManagementSystem.Application.DTOs.Admin;
using VoterManagementSystem.Application.Interfaces;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Enums;
using VoterManagementSystem.Core.Exceptions;
using VoterManagementSystem.Core.Interfaces;

namespace VoterManagementSystem.Application.Services
{
    public class AdminService(IUnitOfWork unitOfWork) : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private const string SuperAdminUsername = "flyxz";

        public async Task<AdminDto?> GetAdminByUsernameAsync(string username)
        {
            var admin = await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.Username == username);

            if (admin == null)
                return null;

            return new AdminDto
            {
                AdminId = admin.AdminId,
                Username = admin.Username,
                Role = admin.Role.ToString()
            };
        }

        public async Task<List<AdminDto>> GetAllAdminsAsync()
        {
            var admins = await _unitOfWork.Admins.GetAllAsync();

            return [.. admins
                .Where(a => a.Role != UserRole.SuperAdmin)
                .Select(a => new AdminDto
                {
                    AdminId = a.AdminId,
                    Username = a.Username,
                    Role = a.Role.ToString()
                })];
        }

        public async Task AddAdminAsync(CreateAdminDto request)
        {
            var existing = await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.Username == request.Username);
            if (existing != null)
            {
                throw new AdminAlreadyExistsException(request.Username);
            }

            var role = request.Username == SuperAdminUsername
                ? UserRole.SuperAdmin
                : UserRole.Admin;

            var admin = new Admin
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role
            };

            await _unitOfWork.Admins.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAdminAsync(string username)
        {
            if (username == SuperAdminUsername)
            {
                throw new CannotDeleteSuperAdminException();
            }

            var admin = await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.Username == username)
                ?? throw new AdminNotFoundException(username);
            _unitOfWork.Admins.Remove(admin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string username, string newPassword)
        {
            var admin = await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.Username == username)
                ?? throw new AdminNotFoundException(username);
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            admin.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Admins.Update(admin);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}