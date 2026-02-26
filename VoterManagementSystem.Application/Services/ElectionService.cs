using Microsoft.EntityFrameworkCore;
using VoterManagementSystem.Application.DTOs.Election;
using VoterManagementSystem.Application.Interfaces;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Enums;
using VoterManagementSystem.Core.Exceptions;
using VoterManagementSystem.Core.Interfaces;

namespace VoterManagementSystem.Application.Services
{
    public class ElectionService(IUnitOfWork unitOfWork) : IElectionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<ElectionDto>> GetAllElectionsAsync()
        {
            var elections = await _unitOfWork.Elections.GetAllAsync();

            var electionDtos = new List<ElectionDto>();

            foreach (var election in elections)
            {
                var voteCount = await _unitOfWork.Votes.CountAsync(v => v.ElectionId == election.ElectionId);
                var partyCount = await _unitOfWork.PartiesInElections.CountAsync(pie => pie.ElectionId == election.ElectionId);

                electionDtos.Add(new ElectionDto
                {
                    ElectionId = election.ElectionId,
                    ElectionCode = election.ElectionCode,
                    Status = election.Status.ToString(),
                    Winner = election.Winner,
                    TotalVotes = voteCount,
                    PartyCount = partyCount
                });
            }

            return electionDtos;
        }

        public async Task<ElectionDto?> GetElectionByCodeAsync(string electionCode)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode);

            if (election == null)
                return null;

            var voteCount = await _unitOfWork.Votes.CountAsync(v => v.ElectionId == election.ElectionId);
            var partyCount = await _unitOfWork.PartiesInElections.CountAsync(pie => pie.ElectionId == election.ElectionId);

            return new ElectionDto
            {
                ElectionId = election.ElectionId,
                ElectionCode = election.ElectionCode,
                Status = election.Status.ToString(),
                Winner = election.Winner,
                TotalVotes = voteCount,
                PartyCount = partyCount
            };
        }

        public async Task RegisterElectionAsync(string electionCode)
        {
            var existing = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode);
            if (existing != null)
            {
                throw new ElectionAlreadyExistsException(electionCode);
            }

            var election = new Election
            {
                ElectionCode = electionCode,
                Status = ElectionStatus.Registered,
                Winner = "No Winner",
                DetailedResult = "Not declared yet"
            };

            await _unitOfWork.Elections.AddAsync(election);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddPartyToElectionAsync(string electionCode, string partyName)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode)
                ?? throw new ElectionNotFoundException(electionCode);
            if (election.Status != ElectionStatus.Registered)
            {
                throw new ElectionNotRegisteredException(electionCode);
            }

            var party = await _unitOfWork.Parties.FirstOrDefaultAsync(p => p.PartyName == partyName)
                ?? throw new PartyNotFoundException(partyName);
            var existingPartyInElection = await _unitOfWork.PartiesInElections
                .FirstOrDefaultAsync(pie => pie.ElectionId == election.ElectionId && pie.PartyId == party.PartyId);

            if (existingPartyInElection != null)
            {
                throw new PartyAlreadyInElectionException(electionCode, partyName);
            }

            var partyInElection = new PartyInElection
            {
                ElectionId = election.ElectionId,
                PartyId = party.PartyId
            };

            await _unitOfWork.PartiesInElections.AddAsync(partyInElection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePartyFromElectionAsync(string electionCode, string partyName)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode)
                ?? throw new ElectionNotFoundException(electionCode);
            if (election.Status != ElectionStatus.Registered)
            {
                throw new ElectionNotRegisteredException(electionCode);
            }

            var party = await _unitOfWork.Parties.FirstOrDefaultAsync(p => p.PartyName == partyName)
                ?? throw new PartyNotFoundException(partyName);
            var partyInElection = await _unitOfWork.PartiesInElections
                .FirstOrDefaultAsync(pie => pie.ElectionId == election.ElectionId && pie.PartyId == party.PartyId)
                ?? throw new PartyNotInElectionException(electionCode, partyName);
            _unitOfWork.PartiesInElections.Remove(partyInElection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<string>> GetPartiesForElectionAsync(string electionCode)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode);
            if (election == null)
            {
                return new List<string>();
            }

            var partiesInElection = await _unitOfWork.PartiesInElections
                .FindAsync(pie => pie.ElectionId == election.ElectionId);

            var partyNames = new List<string>();
            foreach (var pie in partiesInElection)
            {
                var party = await _unitOfWork.Parties.GetByIdAsync(pie.PartyId);
                if (party != null)
                {
                    partyNames.Add(party.PartyName);
                }
            }

            return partyNames;
        }

        public async Task StartElectionAsync(string electionCode)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode)
                ?? throw new ElectionNotFoundException(electionCode);
            if (election.Status != ElectionStatus.Registered)
            {
                throw new ElectionNotRegisteredException(electionCode);
            }

            var partyCount = await _unitOfWork.PartiesInElections.CountAsync(pie => pie.ElectionId == election.ElectionId);
            if (partyCount == 0)
            {
                throw new NoPartiesInElectionException(electionCode);
            }

            election.Status = ElectionStatus.Started;
            election.StartedAt = DateTime.UtcNow;
            election.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Elections.Update(election);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task StopElectionAsync(string electionCode)
        {
            var election = await _unitOfWork.Elections.FirstOrDefaultAsync(e => e.ElectionCode == electionCode)
                ?? throw new ElectionNotFoundException(electionCode);
            if (election.Status != ElectionStatus.Started)
            {
                throw new ElectionNotStartedException(electionCode);
            }

            var partiesInElection = await _unitOfWork.PartiesInElections
                .FindAsync(pie => pie.ElectionId == election.ElectionId);

            var voteCounts = new List<(string PartyName, int Votes)>();

            foreach (var pie in partiesInElection)
            {
                var party = await _unitOfWork.Parties.GetByIdAsync(pie.PartyId);
                if (party != null)
                {
                    var voteCount = await _unitOfWork.Votes
                        .CountAsync(v => v.ElectionId == election.ElectionId && v.PartyId == party.PartyId);

                    voteCounts.Add((party.PartyName, voteCount));
                }
            }

            string winner = "No Winner";
            if (voteCounts.Count == 1)
            {
                winner = voteCounts[0].PartyName;
            }
            else if (voteCounts.Count > 1)
            {
                var maxVotes = voteCounts.Max(v => v.Votes);
                var winnersCount = voteCounts.Count(v => v.Votes == maxVotes);

                if (winnersCount == 1)
                {
                    winner = voteCounts.First(v => v.Votes == maxVotes).PartyName;
                }
            }

            var detailedResult = string.Join(";", voteCounts.Select(v => $"{v.PartyName} {v.Votes}"));

            election.Status = ElectionStatus.Ended;
            election.Winner = winner;
            election.DetailedResult = detailedResult;
            election.EndedAt = DateTime.UtcNow;
            election.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Elections.Update(election);

            foreach (var pie in partiesInElection)
            {
                _unitOfWork.PartiesInElections.Remove(pie);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}