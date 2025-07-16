using JobHub.Data;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHub.repositories
{
    public class ProfileRepository :IProfileReposotity
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<EndUser?> GetEndUserByIdAsync(string userId)
        {
            return await _context.EndUsers
                .Include(e => e.EducationList)
                .Include(e => e.ExperienceList)
                .Include(e => e.Skills)
                .Include(e => e.Languages)
                .FirstOrDefaultAsync(e => e.Id == userId);
        }
        public async Task<bool> UpdateEndUserAsync(EndUser endUser)
        {
            _context.EndUsers.Update(endUser);
            return await _context.SaveChangesAsync() > 0;
        }
 

    }
}
