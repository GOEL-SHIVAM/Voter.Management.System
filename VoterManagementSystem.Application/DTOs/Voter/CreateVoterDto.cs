namespace VoterManagementSystem.Application.DTOs.Voter
{
    public class CreateVoterDto
    {
        public string Aadhar { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}
