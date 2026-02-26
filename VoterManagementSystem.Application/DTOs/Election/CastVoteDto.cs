namespace VoterManagementSystem.Application.DTOs.Election
{
    public class CastVoteDto
    {
        public string ElectionCode { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
    }
}
