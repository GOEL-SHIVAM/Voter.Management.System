namespace VoterManagementSystem.Application.DTOs.Voter
{
    public class VoteHistoryDto
    {
        public string ElectionCode { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public DateTime VotedAt { get; set; }
    }
}
