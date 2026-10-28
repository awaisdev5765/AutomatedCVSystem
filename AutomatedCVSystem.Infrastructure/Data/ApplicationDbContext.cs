using AutomatedCVSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AutomatedCVSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<JobDescription> JobDescriptions { get; set; }
        public DbSet<GeneratedCV> GeneratedCVs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserProfile configuration
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(50);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.ProfessionalSummary).HasMaxLength(1000);
                entity.Property(e => e.LinkedInProfile).HasMaxLength(1000);
                entity.Property(e => e.GitHubProfile).HasMaxLength(1000);
                entity.Property(e => e.PortfolioWebsite).HasMaxLength(1000);
                
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Education configuration
            modelBuilder.Entity<Education>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Institution).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Degree).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FieldOfStudy).HasMaxLength(200);
                entity.Property(e => e.Grade).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Location).HasMaxLength(100);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany(u => u.Educations)
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // WorkExperience configuration
            modelBuilder.Entity<WorkExperience>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.JobTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Achievements).HasMaxLength(2000);
                entity.Property(e => e.Department).HasMaxLength(200);
                entity.Property(e => e.EmploymentType).HasMaxLength(100);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany(u => u.WorkExperiences)
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Skill configuration
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.ProficiencyLevel).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany(u => u.Skills)
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Certification configuration
            modelBuilder.Entity<Certification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IssuingOrganization).HasMaxLength(200);
                entity.Property(e => e.CredentialId).HasMaxLength(100);
                entity.Property(e => e.CredentialUrl).HasMaxLength(1000);
                entity.Property(e => e.Description).HasMaxLength(1000);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany(u => u.Certifications)
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Project configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Technologies).HasMaxLength(2000);
                entity.Property(e => e.ProjectUrl).HasMaxLength(1000);
                entity.Property(e => e.GitHubUrl).HasMaxLength(1000);
                entity.Property(e => e.Role).HasMaxLength(200);
                entity.Property(e => e.Achievements).HasMaxLength(2000);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany(u => u.Projects)
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // JobDescription configuration
            modelBuilder.Entity<JobDescription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JobTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Location).HasMaxLength(1000);
                entity.Property(e => e.FullDescription).IsRequired();
                entity.Property(e => e.ExtractedSkills).HasMaxLength(2000);
                entity.Property(e => e.ExtractedRequirements).HasMaxLength(2000);
                entity.Property(e => e.ExtractedExperience).HasMaxLength(1000);
                entity.Property(e => e.ExtractedEducation).HasMaxLength(1000);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany()
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // GeneratedCV configuration
            modelBuilder.Entity<GeneratedCV>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FileFormat).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TemplateType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FilePath).IsRequired();
                entity.Property(e => e.MatchingScore).HasMaxLength(2000);
                entity.Property(e => e.HighlightedSkills).HasMaxLength(2000);
                entity.Property(e => e.HighlightedExperience).HasMaxLength(2000);
                
                entity.HasOne(e => e.UserProfile)
                    .WithMany()
                    .HasForeignKey(e => e.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.JobDescription)
                    .WithMany()
                    .HasForeignKey(e => e.JobDescriptionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
