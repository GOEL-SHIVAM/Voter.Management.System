using VoterManagementSystem.Application.DTOs.Party;
using VoterManagementSystem.Application.Interfaces;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Exceptions;
using VoterManagementSystem.Core.Interfaces;

namespace VoterManagementSystem.Application.Services
{
    public class PartyService(IUnitOfWork unitOfWork) : IPartyService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<PartyDto>> GetAllPartiesAsync()
        {
            var parties = await _unitOfWork.Parties.GetAllAsync();

            return [.. parties.Select(p => new PartyDto
            {
                PartyId = p.PartyId,
                PartyName = p.PartyName
            })];
        }

        public async Task<PartyDto?> GetPartyByNameAsync(string partyName)
        {
            var party = await _unitOfWork.Parties.FirstOrDefaultAsync(p => p.PartyName == partyName);

            if (party == null)
                return null;

            return new PartyDto
            {
                PartyId = party.PartyId,
                PartyName = party.PartyName
            };
        }

        public async Task AddPartyAsync(CreatePartyDto request)
        {
            var existing = await _unitOfWork.Parties.FirstOrDefaultAsync(p => p.PartyName == request.PartyName);
            if (existing != null)
            {
                throw new PartyAlreadyExistsException(request.PartyName);
            }

            var party = new Party
            {
                PartyName = request.PartyName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _unitOfWork.Parties.AddAsync(party);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePartyAsync(string partyName)
        {
            var party = await _unitOfWork.Parties.FirstOrDefaultAsync(p => p.PartyName == partyName)
                ?? throw new PartyNotFoundException(partyName);
            _unitOfWork.Parties.Remove(party);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}