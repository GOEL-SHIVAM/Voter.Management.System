namespace VoterManagementSystem.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role, string userId);
        bool ValidateToken(string token);
    }
}