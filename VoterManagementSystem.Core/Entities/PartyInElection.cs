using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoterManagementSystem.Core.Entities
{
    public class PartyInElection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ElectionId { get; set; }

        [Required]
        public int PartyId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ElectionId))]
        public virtual Election Election { get; set; } = null!;

        [ForeignKey(nameof(PartyId))]
        public virtual Party Party { get; set; } = null!;
    }
}
