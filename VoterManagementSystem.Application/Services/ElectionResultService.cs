using VoterManagementSystem.Application.DTOs.Election;
using VoterManagementSystem.Application.Interfaces;
using VoterManagementSystem.Core.Enums;
using VoterManagementSystem.Core.Exceptions;
using VoterManagementSystem.Core.Interfaces;

namespace VoterManagementSystem.Application.Services
{
    public class ElectionResultService(IUnitOfWork unitOfWork) : IElectionResultService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ElectionResultDto> GetResultsAsync(string electionCode)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode);
            if (election == null)
            {
                throw new ElectionNotFoundException(electionCode);
            }

            if (election.Status != ElectionStatus.Ended)
            {
                throw new ElectionNotEndedException(electionCode);
            }

            var partyResults = new List<PartyVoteCount>();

            if (!string.IsNullOrWhiteSpace(election.DetailedResult))
            {
                var results = election.DetailedResult.Split(';', StringSplitOptions.RemoveEmptyEntries);
                var totalVotes = 0;

                foreach (var result in results)
                {
                    var parts = result.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && int.TryParse(parts[^1], out int votes))
                    {
                        totalVotes += votes;
                    }
                }

                foreach (var result in results)
                {
                    var parts = result.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && int.TryParse(parts[^1], out int votes))
                    {
                        var partyName = string.Join(" ", parts[..^1]);
                        var percentage = totalVotes > 0 ? (votes * 100.0 / totalVotes) : 0;

                        partyResults.Add(new PartyVoteCount
                        {
                            PartyName = partyName,
                            Votes = votes,
                            Percentage = Math.Round(percentage, 2)
                        });
                    }
                }
            }

            return new ElectionResultDto
            {
                ElectionCode = election.ElectionCode,
                Winner = election.Winner,
                PartyResults = [.. partyResults.OrderByDescending(p => p.Votes)]
            };
        }
    }
}