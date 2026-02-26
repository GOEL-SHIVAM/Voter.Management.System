using VoterManagementSystem.Application.DTOs.Party;

namespace VoterManagementSystem.Application.Interfaces
{
    public interface IPartyService
    {
        Task<List<PartyDto>> GetAllPartiesAsync();
        Task<PartyDto?> GetPartyByNameAsync(string partyName);
        Task AddPartyAsync(CreatePartyDto request);
        Task DeletePartyAsync(string partyName);
    }
}