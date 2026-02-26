using System.ComponentModel.DataAnnotations;
using VoterManagementSystem.Core.Enums;

namespace VoterManagementSystem.Core.Entities
{
    public class Election
    {
        [Key]
        public int ElectionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ElectionCode { get; set; } = string.Empty;

        [Required]
        public ElectionStatus Status { get; set; } = ElectionStatus.Registered;

        [MaxLength(100)]
        public string Winner { get; set; } = "No Winner";

        public string? DetailedResult { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<PartyInElection> PartiesInElections { get; set; } = [];
        public virtual ICollection<Vote> Votes { get; set; } = [];
    }
}
