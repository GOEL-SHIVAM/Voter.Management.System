using System.ComponentModel.DataAnnotations;

namespace VoterManagementSystem.Core.Entities
{
    public class Party
    {
        [Key]
        public int PartyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<PartyInElection> PartiesInElections { get; set; } = [];
        public virtual ICollection<Vote> Votes { get; set; } = [];
    }
}
