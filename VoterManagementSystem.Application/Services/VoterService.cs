using Microsoft.EntityFrameworkCore;
using VoterManagementSystem.Application.DTOs.Voter;
using VoterManagementSystem.Application.Interfaces;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Enums;
using VoterManagementSystem.Core.Exceptions;
using VoterManagementSystem.Core.Interfaces;

namespace VoterManagementSystem.Application.Services
{
    public class VoterService(IUnitOfWork unitOfWork) : IVoterService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<VoterDto>> GetAllVotersAsync()
        {
            var voters = await _unitOfWork.Voters.GetAllAsync();

            return [.. voters.Select(v => new VoterDto
            {
                VoterId = v.VoterId,
                Aadhar = v.Aadhar,
                Name = v.Name,
                BirthDate = v.BirthDate,
                Age = CalculateAge(v.BirthDate)
            })];
        }

        public async Task<VoterDto> GetVoterByAadharAsync(string aadhar)
        {
            var voter = await _unitOfWork.Voters.FirstOrDefaultAsync(v => v.Aadhar == aadhar)
                ?? throw new VoterNotFoundException(aadhar);

            return new VoterDto
            {
                VoterId = voter.VoterId,
                Aadhar = voter.Aadhar,
                Name = voter.Name,
                BirthDate = voter.BirthDate,
                Age = CalculateAge(voter.BirthDate)
            };
        }

        public async Task CreateVoterAsync(CreateVoterDto request)
        {
            var exists = await _unitOfWork.Voters.ExistsAsync(v => v.Aadhar == request.Aadhar);
            if (exists)
                throw new VoterAlreadyExistsException(request.Aadhar);

            var age = CalculateAge(request.BirthDate);
            if (age < 18)
                throw new VoterUnderAgeException(request.BirthDate);

            var voter = new Voter
            {
                Aadhar = request.Aadhar,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name,
                BirthDate = request.BirthDate
            };

            await _unitOfWork.Voters.AddAsync(voter);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateVoterAsync(string aadhar, UpdateVoterDto request)
        {
            var voter = await _unitOfWork.Voters.FirstOrDefaultAsync(v => v.Aadhar == aadhar)
                ?? throw new VoterNotFoundException(aadhar);

            if (!string.IsNullOrWhiteSpace(request.Name))
                voter.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Password))
                voter.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            voter.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Voters.Update(voter);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVoterAsync(string aadhar)
        {
            var voter = await _unitOfWork.Voters.FirstOrDefaultAsync(v => v.Aadhar == aadhar)
                ?? throw new VoterNotFoundException(aadhar);

            _unitOfWork.Voters.Remove(voter);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string aadhar, string newPassword)
        {
            var voter = await _unitOfWork.Voters.FirstOrDefaultAsync(v => v.Aadhar == aadhar)
                ?? throw new VoterNotFoundException(aadhar);

            voter.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            voter.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Voters.Update(voter);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CastVoteAsync(string aadhar, string electionCode, string partyName)
        {
            var voter = await _unitOfWork.Voters.FirstOrDefaultAsync(v => v.Aadhar == aadhar)
                ?? throw new VoterNotFoundException(aadhar);

            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode)
                ?? throw new ElectionNotFoundException(electionCode);

            if (election.Status != ElectionStatus.Started)
                throw new ElectionNotStartedException(electionCode);

            var party = await _unitOfWork.Parties.FirstOrDefaultAsync(p => p.PartyName == partyName)
                ?? throw new PartyNotFoundException(partyName);

            var partyInElection = await _unitOfWork.PartiesInElections
                .FirstOrDefaultAsync(pie => pie.ElectionId == election.ElectionId && pie.PartyId == party.PartyId) 
                ?? throw new PartyNotInElectionException(electionCode, partyName);
            var hasVoted = await _unitOfWork.Votes
                .ExistsAsync(v => v.VoterId == voter.VoterId && v.ElectionId == election.ElectionId);

            if (hasVoted)
                throw new VoterAlreadyVotedException(aadhar, electionCode);

            var vote = new Vote
            {
                VoterId = voter.VoterId,
                ElectionId = election.ElectionId,
                PartyId = party.PartyId
            };

            await _unitOfWork.Votes.AddAsync(vote);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<VoteHistoryDto>> GetRecentVotesAsync(string aadhar, int count)
        {
            var voter = await _unitOfWork.Voters
                .FirstOrDefaultAsync(v => v.Aadhar == aadhar)
                ?? throw new VoterNotFoundException(aadhar);

            var votes = await _unitOfWork.Votes
                .FindAsync(v => v.VoterId == voter.VoterId);

            var result = new List<VoteHistoryDto>();

            foreach (var v in votes.Take(count))
            {
                var election = await _unitOfWork.Elections
                    .FirstOrDefaultAsync(e => e.ElectionId == v.ElectionId);

                var party = await _unitOfWork.Parties
                    .FirstOrDefaultAsync(p => p.PartyId == v.PartyId);

                result.Add(new VoteHistoryDto
                {
                    ElectionCode = election?.ElectionCode ?? "Unknown",
                    PartyName = party?.PartyName ?? "Unknown",
                    VotedAt = v.VotedAt
                });
            }

            return result;
        }


        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}