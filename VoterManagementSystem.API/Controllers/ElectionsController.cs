using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Election;
using VoterManagementSystem.Application.DTOs.Party;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    public class ElectionsController(
        IElectionService electionService,
        IValidator<CreateElectionDto> createElectionValidator,
        IValidator<AddPartyToElectionDto> addPartyValidator) : BaseApiController
    {
        private readonly IElectionService _electionService = electionService;
        private readonly IValidator<CreateElectionDto> _createElectionValidator = createElectionValidator;
        private readonly IValidator<AddPartyToElectionDto> _addPartyValidator = addPartyValidator;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ElectionDto>>> GetAllElections()
        {
            var elections = await _electionService.GetAllElectionsAsync();
            return Ok(elections);
        }

        [HttpGet("{electionCode}")]
        [AllowAnonymous]
        public async Task<ActionResult<ElectionDto>> GetElection(string electionCode)
        {
            var election = await _electionService.GetElectionByCodeAsync(electionCode);

            if (election == null)
            {
                return NotFound(new { message = $"Election '{electionCode}' not found" });
            }

            return Ok(election);
        }

        [HttpGet("{electionCode}/parties")]
        [AllowAnonymous]
        public async Task<ActionResult<List<string>>> GetElectionParties(string electionCode)
        {
            var parties = await _electionService.GetPartiesForElectionAsync(electionCode);
            return Ok(parties);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> RegisterElection([FromBody] CreateElectionDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _createElectionValidator,
                async () =>
                {
                    await _electionService.RegisterElectionAsync(request.ElectionCode);
                }
            );
        }

        [HttpPost("{electionCode}/parties")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> AddPartyToElection(
            string electionCode,
            [FromBody] AddPartyToElectionDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _addPartyValidator,
                async () =>
                {
                    await _electionService.AddPartyToElectionAsync(electionCode, request.PartyName);
                }
            );
        }

        [HttpDelete("{electionCode}/parties/{partyName}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> RemovePartyFromElection(string electionCode, string partyName)
        {
            await _electionService.DeletePartyFromElectionAsync(electionCode, partyName);
            return Ok(new { message = $"Party '{partyName}' removed from election '{electionCode}'" });
        }

        [HttpPut("{electionCode}/start")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> StartElection(string electionCode)
        {
            await _electionService.StartElectionAsync(electionCode);
            return Ok(new { message = $"Election '{electionCode}' started successfully" });
        }

        [HttpPut("{electionCode}/stop")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> StopElection(string electionCode)
        {
            await _electionService.StopElectionAsync(electionCode);
            return Ok(new { message = $"Election '{electionCode}' stopped successfully. Results calculated." });
        }
    }
}