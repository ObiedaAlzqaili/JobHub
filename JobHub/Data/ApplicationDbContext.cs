using JobHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SkillType>().HasData(
               
                new SkillType { Id = 1, Name = "Hard Skills" },
                new SkillType { Id = 2, Name = "SoftSkills" }

            );

            modelBuilder.Entity<SkillLevel>().HasData(
                new SkillLevel { Id = 1, Level = "Beginner" },
                new SkillLevel { Id = 2, Level = "Intermediate" },
                new SkillLevel { Id = 3, Level = "Expert" }
            );
        }
    }
}
