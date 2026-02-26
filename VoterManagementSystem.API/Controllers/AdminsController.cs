using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Admin;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminsController(
        IAdminService adminService,
        IValidator<CreateAdminDto> createAdminValidator,
        IValidator<ChangePasswordDto> changePasswordValidator) : BaseApiController
    {
        private readonly IAdminService _adminService = adminService;
        private readonly IValidator<CreateAdminDto> _createAdminValidator = createAdminValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator = changePasswordValidator;

        [HttpGet]
        public async Task<ActionResult<List<AdminDto>>> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AdminDto>> GetAdmin(string username)
        {
            var admin = await _adminService.GetAdminByUsernameAsync(username);

            if (admin == null)
            {
                return NotFound(new { message = $"Admin '{username}' not found" });
            }

            return Ok(admin);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAdmin([FromBody] CreateAdminDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _createAdminValidator,
                async () =>
                {
                    await _adminService.AddAdminAsync(request);
                }
            );
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteAdmin(string username)
        {
            await _adminService.DeleteAdminAsync(username);
            return Ok(new { message = $"Admin '{username}' deleted successfully" });
        }

        [HttpPut("change-password")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _changePasswordValidator,
                async () =>
                {
                    var username = User.Identity?.Name;
                    if (string.IsNullOrEmpty(username))
                    {
                        throw new UnauthorizedAccessException("User not authenticated");
                    }

                    await _adminService.ChangePasswordAsync(username, request.NewPassword);
                }
            );
        }

        [HttpPut("{username}/change-password")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> ChangeAdminPassword(string username, [FromBody] ChangePasswordDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _changePasswordValidator,
                async () =>
                {
                    await _adminService.ChangePasswordAsync(username, request.NewPassword);
                }
            );
        }
    }
}
