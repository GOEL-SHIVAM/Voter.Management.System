namespace VoterManagementSystem.Application.DTOs.Election
{
    public class PartyVoteCount
    {
        public string PartyName { get; set; } = string.Empty;
        public int Votes { get; set; }
        public double Percentage { get; set; }
    }
}
