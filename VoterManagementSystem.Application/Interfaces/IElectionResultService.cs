using VoterManagementSystem.Application.DTOs.Election;

namespace VoterManagementSystem.Application.Interfaces
{
    public interface IElectionResultService
    {
        Task<ElectionResultDto> GetResultsAsync(string electionCode);
    }
}