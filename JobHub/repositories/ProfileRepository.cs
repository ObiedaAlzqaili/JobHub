using JobHub.Data;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace JobHub.repositories
{
    public class ProfileRepository : IProfileReposotity
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> CreateEndUserAsync(EndUser endUser)
        {
            try
            {
                _context.EndUsers.Add(endUser);
                _context.SaveChanges();
                return Task.FromResult(true);

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return Task.FromResult(false);
            }



        }

        public async Task<EndUser?> GetEndUserByIdAsync(string userId)
        {
            try
            {
                return await _context.EndUsers
             .Include(e => e.EducationList)
             .Include(e => e.ExperienceList)
             .Include(e => e.Skills)
             .Include(e => e.Languages)
             .FirstOrDefaultAsync(e => e.Id == userId);

            }
            catch (Exception ex)
            {

                throw new Exception($"User can't found with this Id : {userId} cuz {ex} ");

            }
        }
        public async Task<bool> UpdateEndUserAsync(EndUser endUser)
        {
            try
            {
                _context.EndUsers.Update(endUser);
                return await _context.SaveChangesAsync() > 0;

            }
            catch (Exception ex)
            {

                return false;

            }


        }
    }
}
