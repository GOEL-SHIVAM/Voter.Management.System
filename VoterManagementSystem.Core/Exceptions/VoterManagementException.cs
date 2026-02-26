namespace VoterManagementSystem.Core.Exceptions
{
    public class VoterManagementException : Exception
    {
        public VoterManagementException() : base() { }
        public VoterManagementException(string message) : base(message) { }
        public VoterManagementException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}