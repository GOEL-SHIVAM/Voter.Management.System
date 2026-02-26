namespace VoterManagementSystem.Core.Exceptions
{
    public class PartyAlreadyExistsException(string partyName) 
        : VoterManagementException($"Party '{partyName}' already exists.")
    { }

    public class PartyNotFoundException(string partyName) 
        : VoterManagementException($"Party '{partyName}' not found.")
    { }

    public class PartyAlreadyInElectionException(string electionCode, string partyName) 
        : VoterManagementException($"Party '{partyName}' is already in election '{electionCode}'.")
    { }

    public class PartyNotInElectionException(string electionCode, string partyName) 
        : VoterManagementException($"Party '{partyName}' is not in election '{electionCode}'.")
    { }
}