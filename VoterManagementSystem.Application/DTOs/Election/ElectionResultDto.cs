namespace VoterManagementSystem.Application.DTOs.Election
{
    public class ElectionResultDto
    {
        public string ElectionCode { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public List<PartyVoteCount> PartyResults { get; set; } = new();
    }

}
