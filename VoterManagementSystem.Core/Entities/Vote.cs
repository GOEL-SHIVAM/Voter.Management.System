using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoterManagementSystem.Core.Entities
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }

        [Required]
        public int VoterId { get; set; }

        [Required]
        public int ElectionId { get; set; }

        [Required]
        public int PartyId { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(VoterId))]
        public virtual Voter Voter { get; set; } = null!;

        [ForeignKey(nameof(ElectionId))]
        public virtual Election Election { get; set; } = null!;

        [ForeignKey(nameof(PartyId))]
        public virtual Party Party { get; set; } = null!;
    }
}
