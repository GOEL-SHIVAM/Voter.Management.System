using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Election;
using VoterManagementSystem.Application.DTOs.Voter;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Voter")]
    public class VotesController(
        IVoterService voterService,
        IValidator<CastVoteDto> castVoteValidator) : BaseApiController
    {
        private readonly IVoterService _voterService = voterService;
        private readonly IValidator<CastVoteDto> _castVoteValidator = castVoteValidator;

        [HttpPost]
        public async Task<ActionResult> CastVote([FromBody] CastVoteDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _castVoteValidator,
                async () =>
                {
                    var aadhar = User.Identity?.Name;
                    if (string.IsNullOrEmpty(aadhar))
                    {
                        throw new UnauthorizedAccessException("User not authenticated");
                    }

                    await _voterService.CastVoteAsync(aadhar, request.ElectionCode, request.PartyName);
                }
            );
        }

        [HttpGet("my-history")]
        public async Task<ActionResult<List<VoteHistoryDto>>> GetMyVoteHistory([FromQuery] int count = 10)
        {
            var aadhar = User.Identity?.Name;
            if (string.IsNullOrEmpty(aadhar))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            if (count < 1 || count > 100)
            {
                return BadRequest(new { message = "Count must be between 1 and 100" });
            }

            var history = await _voterService.GetRecentVotesAsync(aadhar, count);
            return Ok(history);
        }

        [HttpGet("my-vote-count")]
        public async Task<ActionResult<object>> GetMyVoteCount()
        {
            var aadhar = User.Identity?.Name;
            if (string.IsNullOrEmpty(aadhar))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var votes = await _voterService.GetRecentVotesAsync(aadhar, 1000); // Get all votes

            return Ok(new
            {
                aadhar = aadhar,
                totalVotesCast = votes.Count,
                electionsVotedIn = votes.Select(v => v.ElectionCode).Distinct().Count()
            });
        }
    }
}