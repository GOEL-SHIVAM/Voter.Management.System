using System.ComponentModel.DataAnnotations;
using VoterManagementSystem.Core.Enums;

namespace VoterManagementSystem.Core.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Admin;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
