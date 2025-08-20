using JobHub.Data;
using JobHub.DTOs.Job;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace JobHub.repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateJobApplication(JobApplicationDto application)
        {
            try
            {
                if (application == null)
                {
                    throw new ArgumentNullException(nameof(application), "Application cannot be null");
                }

                // Check for existing application (redundant safety check)
                var existingApp = await _context.JobApplications
                    .FirstOrDefaultAsync(ja => ja.JobPostId == application.JobPostId &&
                                              ja.EndUserId == application.EndUserId);

                if (existingApp != null)
                {
                    return false; // Application already exists
                }

                var jobApplication = new JobApplication
                {
                    JobPostId = application.JobPostId,
                    Name = application.ApplicantName,
                    Email = application.ApplicantEmail,
                    PhoneNumber = application.PhoneNumber,
                    ResumeBase64 = application.ResumeBase64,
                    ResumeName = application.ResumeFileName,
                    ResumeType = application.ResumeFileType,
                    EndUserId = application.EndUserId,
                    AppliedOn = DateTime.UtcNow,
                    Status = "Pending"
                };

                _context.JobApplications.Add(jobApplication);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public async Task<IEnumerable<JobPostSearchDto>> GetAllJobsAsync()
        {
            try
            {
                return await _context.JobPosts
                    .Select(j => new JobPostSearchDto
                    {
                        Id = j.Id,
                        Title = j.Title,
                        PostedAt = j.PostedAt,
                        CompanyName = j.Company.CompanyName,
                        Location = j.Location
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
        
                throw new Exception("An error occurred while retrieving all jobs.", ex);
            }
        }

        public async Task<IEnumerable<JobPostSearchDto>> GetFiveJobAsync()
        {
            try
            {
                return await _context.JobPosts
                    .Select(j => new JobPostSearchDto
                    {
                        Id = j.Id,
                        Title = j.Title,
                        PostedAt = j.PostedAt,
                        CompanyName = j.Company.CompanyName,
                        Location = j.Location
                    })
                    .Take(5)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while retrieving the top five jobs.", ex);
            }
        }

        public async Task<JobPost> GetJobByIdAsync(int jobId)
        {
            try
            {
                return await _context.JobPosts
                    .Where(j => j.Id == jobId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception($"An error occurred while retrieving the job with ID {jobId}.", ex);
            }
        }

        public async Task<IEnumerable<JobPostSearchDto>> GetJobsByCompanyAsync(int id)
        {
            //try
            //{
            //    return await _context.JobPosts
            //        .Where(j => j.Company.Id == id)
            //        .Select(j => new JobPostSearchDto
            //        {
            //            Id = j.Id,
            //            Title = j.Title,
            //            PostedAt = j.PostedAt,
            //            CompanyName = j.Company.CompanyName,
            //            Location = j.Location
            //        })
            //        .ToListAsync();
            //}
            //catch (Exception ex)
            //{
            //    // Log exception here
            //    throw new Exception($"An error occurred while retrieving jobs for the company with ID {id}.", ex);
            //}
            throw new NotImplementedException("decs");
       }

        public async Task<IEnumerable<JobPostSearchDto>> GetJobsByTitleAsync(string title, string location)
        {
            try
            {
                return await _context.JobPosts
                    .Where(j => j.Title.Contains(title) && j.Location.Contains(location))
                    .Select(j => new JobPostSearchDto
                    {
                        Title = j.Title,
                        PostedAt = j.PostedAt,
                        Id = j.Id,
                        CompanyName = j.Company.CompanyName,
                        Location = j.Location
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception($"An error occurred while retrieving jobs with title '{title}' and location '{location}'.", ex);
            }
        }

       
    }
}
