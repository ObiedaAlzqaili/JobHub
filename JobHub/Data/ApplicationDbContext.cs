using JobHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<Skill> Skills { get; set; } // Assuming you have a Skill model defined
        public DbSet<EndUser> EndUsers { get; set; } // Assuming you have an EndUser model defined

        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<Language> Languages { get; set; }
      

        public DbSet<Resume> Resumes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          

            modelBuilder.Entity<SkillLevel>().HasData(
                new SkillLevel { Id = 1, Level = "Beginner" },
                new SkillLevel { Id = 2, Level = "Intermediate" },
                new SkillLevel { Id = 3, Level = "Expert" }
            );
        }
    }
}
