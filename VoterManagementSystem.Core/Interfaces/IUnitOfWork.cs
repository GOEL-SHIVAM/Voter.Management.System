using VoterManagementSystem.Core.Entities;

namespace VoterManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Admin> Admins { get; }
        IRepository<Voter> Voters { get; }
        IRepository<Party> Parties { get; }
        IRepository<Election> Elections { get; }
        IRepository<PartyInElection> PartiesInElections { get; }
        IRepository<Vote> Votes { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
