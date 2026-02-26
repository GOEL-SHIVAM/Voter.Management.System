namespace VoterManagementSystem.Core.Exceptions
{
    public class AdminAlreadyExistsException(string username) 
        : VoterManagementException($"Admin '{username}' already exists.") 
    { }

    public class AdminNotFoundException(string username) 
        : VoterManagementException($"Admin '{username}' not found.")
    { }

    public class CannotDeleteSuperAdminException : VoterManagementException
    {
        public CannotDeleteSuperAdminException()
            : base("Cannot delete super admin.") { }
    }
}