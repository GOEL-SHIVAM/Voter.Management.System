using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Party;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PartiesController(
        IPartyService partyService,
        IValidator<CreatePartyDto> createPartyValidator) : BaseApiController
    {
        private readonly IPartyService _partyService = partyService;
        private readonly IValidator<CreatePartyDto> _createPartyValidator = createPartyValidator;

        [HttpGet]
        [AllowAnonymous] 
        public async Task<ActionResult<List<PartyDto>>> GetAllParties()
        {
            var parties = await _partyService.GetAllPartiesAsync();
            return Ok(parties);
        }

        [HttpGet("{partyName}")]
        [AllowAnonymous]
        public async Task<ActionResult<PartyDto>> GetParty(string partyName)
        {
            var party = await _partyService.GetPartyByNameAsync(partyName);

            if (party == null)
            {
                return NotFound(new { message = $"Party '{partyName}' not found" });
            }

            return Ok(party);
        }

        [HttpPost]
        public async Task<ActionResult> CreateParty([FromBody] CreatePartyDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _createPartyValidator,
                async () =>
                {
                    await _partyService.AddPartyAsync(request);
                }
            );
        }

        [HttpDelete("{partyName}")]
        public async Task<ActionResult> DeleteParty(string partyName)
        {
            await _partyService.DeletePartyAsync(partyName);
            return Ok(new { message = $"Party '{partyName}' deleted successfully" });
        }
    }
}
