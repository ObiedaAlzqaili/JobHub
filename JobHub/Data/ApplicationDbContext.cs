using JobHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
      
        public DbSet<Skill> Skills { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }

        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<Language> Languages { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Resume> Resumes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        


            
            modelBuilder.Entity<JobApplication>()
            .HasKey(ja => new { ja.EndUserId, ja.JobPostId }); 

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.EndUser)
                .WithMany(eu => eu.JobApplications)
                .HasForeignKey(ja => ja.EndUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.JobPost)
                .WithMany(jp => jp.JobApplications)
                .HasForeignKey(ja => ja.JobPostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
