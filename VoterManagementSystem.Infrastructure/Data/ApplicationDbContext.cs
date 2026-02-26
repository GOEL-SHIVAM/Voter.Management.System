using Microsoft.EntityFrameworkCore;
using VoterManagementSystem.Core.Entities;
using VoterManagementSystem.Core.Enums;

namespace VoterManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : DbContext(options)
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<PartyInElection> PartiesInElections { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin Configuration
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admins");
                entity.HasKey(a => a.AdminId);
                entity.HasIndex(a => a.Username).IsUnique();
                entity.Property(a => a.Role).HasConversion<string>();
            });

            // Voter Configuration
            modelBuilder.Entity<Voter>(entity =>
            {
                entity.ToTable("Voters");
                entity.HasKey(v => v.VoterId);
                entity.HasIndex(v => v.Aadhar).IsUnique();
                entity.Property(v => v.Aadhar).HasMaxLength(12).IsFixedLength();
            });

            // Party Configuration
            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("Parties");
                entity.HasKey(p => p.PartyId);
                entity.HasIndex(p => p.PartyName).IsUnique();
            });

            // Election Configuration
            modelBuilder.Entity<Election>(entity =>
            {
                entity.ToTable("Elections");
                entity.HasKey(e => e.ElectionId);
                entity.HasIndex(e => e.ElectionCode).IsUnique();
                entity.Property(e => e.Status).HasConversion<string>();
            });

            // PartyInElection Configuration
            modelBuilder.Entity<PartyInElection>(entity =>
            {
                entity.ToTable("PartiesInElections");
                entity.HasKey(pie => pie.Id);
                entity.HasIndex(pie => new { pie.ElectionId, pie.PartyId }).IsUnique();

                entity.HasOne(pie => pie.Election)
                    .WithMany(e => e.PartiesInElections)
                    .HasForeignKey(pie => pie.ElectionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pie => pie.Party)
                    .WithMany(p => p.PartiesInElections)
                    .HasForeignKey(pie => pie.PartyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Vote Configuration
            modelBuilder.Entity<Vote>(entity =>
            {
                entity.ToTable("Votes");
                entity.HasKey(v => v.VoteId);
                entity.HasIndex(v => new { v.VoterId, v.ElectionId }).IsUnique();

                entity.HasOne(v => v.Voter)
                    .WithMany(vo => vo.Votes)
                    .HasForeignKey(v => v.VoterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(v => v.Election)
                    .WithMany(e => e.Votes)
                    .HasForeignKey(v => v.ElectionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(v => v.Party)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(v => v.PartyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed Data - Super Admin
            var superAdminPasswordHash = BCrypt.Net.BCrypt.HashPassword("iamsuperadmin");

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = 1,
                    Username = "flyxz",
                    PasswordHash = superAdminPasswordHash,
                    Role = UserRole.SuperAdmin,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}