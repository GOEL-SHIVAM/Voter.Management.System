namespace VoterManagementSystem.Application.DTOs.Election
{
    public class ElectionDto
    {
        public int ElectionId { get; set; }
        public string ElectionCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public int TotalVotes { get; set; }
        public int PartyCount { get; set; }
    }

}
