namespace VoterManagementSystem.Core.Exceptions
{
    public class ElectionAlreadyExistsException(string electionCode)
        : VoterManagementException($"Election '{electionCode}' already exists.")
    { }

    public class ElectionNotFoundException(string electionCode) 
        : VoterManagementException($"Election '{electionCode}' not found.")
    { }

    public class ElectionNotStartedException(string electionCode) 
        : VoterManagementException($"Election '{electionCode}' has not been started.")
    { }

    public class ElectionNotRegisteredException(string electionCode) 
        : VoterManagementException($"Election '{electionCode}' is not in registered state.")
    { }

    public class ElectionNotEndedException(string electionCode) 
        : VoterManagementException($"Election '{electionCode}' has not ended yet.")
    { }

    public class NoPartiesInElectionException(string electionCode) 
        : VoterManagementException($"No parties registered for election '{electionCode}'.")
    { }
}