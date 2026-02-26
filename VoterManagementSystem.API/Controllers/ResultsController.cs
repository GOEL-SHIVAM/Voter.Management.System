using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Election;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController(IElectionResultService resultService) : ControllerBase
    {
        private readonly IElectionResultService _resultService = resultService;

        [HttpGet("{electionCode}")]
        public async Task<ActionResult<ElectionResultDto>> GetResults(string electionCode)
        {
            var results = await _resultService.GetResultsAsync(electionCode);
            return Ok(results);
        }

        [HttpGet("{electionCode}/detailed")]
        public async Task<ActionResult<object>> GetDetailedResults(string electionCode)
        {
            var results = await _resultService.GetResultsAsync(electionCode);

            var totalVotes = results.PartyResults.Sum(p => p.Votes);

            return Ok(new
            {
                electionCode = results.ElectionCode,
                winner = results.Winner,
                totalVotes = totalVotes,
                partyResults = results.PartyResults.Select(p => new
                {
                    partyName = p.PartyName,
                    votes = p.Votes,
                    percentage = p.Percentage,
                    votePercentageDisplay = $"{p.Percentage}%"
                }).OrderByDescending(p => p.votes),
                winnerDetails = results.PartyResults
                    .Where(p => p.PartyName == results.Winner)
                    .Select(p => new
                    {
                        partyName = p.PartyName,
                        votes = p.Votes,
                        percentage = p.Percentage
                    })
                    .FirstOrDefault()
            });
        }

        [HttpGet("{electionCode}/summary")]
        public async Task<ActionResult<object>> GetResultsSummary(string electionCode)
        {
            var results = await _resultService.GetResultsAsync(electionCode);

            var totalVotes = results.PartyResults.Sum(p => p.Votes);
            var winningParty = results.PartyResults.FirstOrDefault(p => p.PartyName == results.Winner);

            return Ok(new
            {
                electionCode = results.ElectionCode,
                status = "Ended",
                winner = results.Winner,
                totalVotes = totalVotes,
                totalParties = results.PartyResults.Count,
                winningVotes = winningParty?.Votes ?? 0,
                winningPercentage = winningParty?.Percentage ?? 0
            });
        }
    }
}