using VoterManagementSystem.Application.DTOs.Election;

namespace VoterManagementSystem.Application.Interfaces
{
    public interface IElectionService
    {
        Task<List<ElectionDto>> GetAllElectionsAsync();
        Task<ElectionDto?> GetElectionByCodeAsync(string electionCode);
        Task RegisterElectionAsync(string electionCode);
        Task AddPartyToElectionAsync(string electionCode, string partyName);
        Task DeletePartyFromElectionAsync(string electionCode, string partyName);
        Task<List<string>> GetPartiesForElectionAsync(string electionCode);
        Task StartElectionAsync(string electionCode);
        Task StopElectionAsync(string electionCode);
    }
}