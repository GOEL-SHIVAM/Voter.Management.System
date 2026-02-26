using Microsoft.EntityFrameworkCore.Storage;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Interfaces;
using VoterManagementSystem.Infrastructure.Data;

namespace VoterManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IRepository<Admin> Admins { get; }
        public IRepository<Voter> Voters { get; }
        public IRepository<Party> Parties { get; }
        public IRepository<Election> Elections { get; }
        public IRepository<PartyInElection> PartiesInElections { get; }
        public IRepository<Vote> Votes { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Admins = new Repository<Admin>(_context);
            Voters = new Repository<Voter>(_context);
            Parties = new Repository<Party>(_context);
            Elections = new Repository<Election>(_context);
            PartiesInElections = new Repository<PartyInElection>(_context);
            Votes = new Repository<Vote>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}