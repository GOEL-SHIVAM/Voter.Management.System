using System.ComponentModel.DataAnnotations;

namespace VoterManagementSystem.Core.Entities
{
    public class Voter
    {
        [Key]
        public int VoterId { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string Aadhar { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Vote> Votes { get; set; } = [];
    }
}
