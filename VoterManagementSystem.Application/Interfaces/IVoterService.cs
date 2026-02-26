using VoterManagementSystem.Application.DTOs.Voter;

namespace VoterManagementSystem.Application.Interfaces
{
    public interface IVoterService
    {
        Task<List<VoterDto>> GetAllVotersAsync();
        Task<VoterDto> GetVoterByAadharAsync(string aadhar);
        Task CreateVoterAsync(CreateVoterDto request);
        Task UpdateVoterAsync(string aadhar, UpdateVoterDto request);
        Task DeleteVoterAsync(string aadhar);
        Task ChangePasswordAsync(string aadhar, string newPassword);
        Task CastVoteAsync(string aadhar, string electionCode, string partyName);
        Task<List<VoteHistoryDto>> GetRecentVotesAsync(string aadhar, int count);
    }
}