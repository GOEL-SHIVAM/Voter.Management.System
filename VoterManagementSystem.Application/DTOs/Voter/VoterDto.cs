namespace VoterManagementSystem.Application.DTOs.Voter
{
    public class VoterDto
    {
        public int VoterId { get; set; }
        public string Aadhar { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
    }

}
