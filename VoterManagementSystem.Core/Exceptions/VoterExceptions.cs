namespace VoterManagementSystem.Core.Exceptions
{
    public class VoterAlreadyExistsException(string aadhar) 
        : VoterManagementException($"Voter with Aadhar '{aadhar}' already exists.")
    { }

    public class VoterNotFoundException(string aadhar) 
        : VoterManagementException($"Voter with Aadhar '{aadhar}' not found.")
    { }

    public class VoterUnderAgeException(DateTime birthDate) 
        : VoterManagementException($"Voter born on {birthDate:dd-MM-yyyy} is under 18 years old.")
    { }

    public class VoterAlreadyVotedException(string aadhar, string electionId) 
        : VoterManagementException($"Voter '{aadhar}' has already voted in election '{electionId}'.")
    { }
}