using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Admin;
using VoterManagementSystem.Application.DTOs.Voter;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class VotersController(
        IVoterService voterService,
        IValidator<CreateVoterDto> createVoterValidator,
        IValidator<UpdateVoterDto> updateVoterValidator,
        IValidator<ChangePasswordDto> changePasswordValidator) : BaseApiController
    {
        private readonly IVoterService _voterService = voterService;
        private readonly IValidator<CreateVoterDto> _createVoterValidator = createVoterValidator;
        private readonly IValidator<UpdateVoterDto> _updateVoterValidator = updateVoterValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator = changePasswordValidator;

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<List<VoterDto>>> GetAllVoters()
        {
            var voters = await _voterService.GetAllVotersAsync();
            return Ok(voters);
        }

        [HttpGet("{aadhar}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<VoterDto>> GetVoter(string aadhar)
        {
            var voter = await _voterService.GetVoterByAadharAsync(aadhar);
            return Ok(voter);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> CreateVoter([FromBody] CreateVoterDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _createVoterValidator,
                async () =>
                {
                    await _voterService.CreateVoterAsync(request);
                }
            );
        }

        [HttpPut("{aadhar}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> UpdateVoter(string aadhar, [FromBody] UpdateVoterDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _updateVoterValidator,
                async () =>
                {
                    await _voterService.UpdateVoterAsync(aadhar, request);
                }
            );
        }

        [HttpDelete("{aadhar}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> DeleteVoter(string aadhar)
        {
            await _voterService.DeleteVoterAsync(aadhar);
            return Ok(new { message = $"Voter with Aadhar '{aadhar}' deleted successfully" });
        }

        [HttpPut("change-password")]
        [Authorize(Roles = "Voter")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _changePasswordValidator,
                async () =>
                {
                    // Get current voter's Aadhar from JWT token
                    var aadhar = User.Identity?.Name;
                    if (string.IsNullOrEmpty(aadhar))
                    {
                        throw new UnauthorizedAccessException("User not authenticated");
                    }

                    await _voterService.ChangePasswordAsync(aadhar, request.NewPassword);
                }
            );
        }

        [HttpGet("my-profile")]
        [Authorize(Roles = "Voter")]
        public async Task<ActionResult<VoterDto>> GetMyProfile()
        {
            var aadhar = User.Identity?.Name;
            if (string.IsNullOrEmpty(aadhar))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var voter = await _voterService.GetVoterByAadharAsync(aadhar);
            return Ok(voter);
        }

        [HttpGet("my-votes")]
        [Authorize(Roles = "Voter")]
        public async Task<ActionResult<List<VoteHistoryDto>>> GetMyVotes([FromQuery] int count = 10)
        {
            var aadhar = User.Identity?.Name;
            if (string.IsNullOrEmpty(aadhar))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var votes = await _voterService.GetRecentVotesAsync(aadhar, count);
            return Ok(votes);
        }
    }
}