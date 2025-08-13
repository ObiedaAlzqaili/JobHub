using JobHub.Data;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace JobHub.repositories
{
    public class ProfileRepository : IProfileReposotity
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEndUserAsync(EndUser updatedUser)
        {
            try
            {
                var existingUser = await _context.EndUsers
                    .Include(u => u.EducationList)
                    .Include(u => u.ExperienceList)
                    .Include(u => u.Skills)
                    .Include(u => u.Languages)
                    .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

                if (existingUser == null)
                {
                    return false; // user not found
                }

                // Update basic fields
                existingUser.FullName = updatedUser.FullName;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.Headline = updatedUser.Headline;
                existingUser.Address = updatedUser.Address;
                existingUser.DayOfBirth = updatedUser.DayOfBirth;
                existingUser.Description = updatedUser.Description;
                existingUser.PersonalImageBase64 = updatedUser.PersonalImageBase64;
                existingUser.PersonalImageType = updatedUser.PersonalImageType;
                existingUser.PersonalImageName = updatedUser.PersonalImageName;
                existingUser.ResumeBase64 = updatedUser.ResumeBase64;
                existingUser.ResumeType = updatedUser.ResumeType;
                existingUser.ResumeName = updatedUser.ResumeName;

                // Clear old lists and replace with new ones
                existingUser.EducationList.Clear();
                existingUser.EducationList.AddRange(updatedUser.EducationList);

                existingUser.ExperienceList.Clear();
                existingUser.ExperienceList.AddRange(updatedUser.ExperienceList);

                existingUser.Skills.Clear();
                existingUser.Skills.AddRange(updatedUser.Skills);

                existingUser.Languages.Clear();
                existingUser.Languages.AddRange(updatedUser.Languages);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // log ex
                return false;
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
