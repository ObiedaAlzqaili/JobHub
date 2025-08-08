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

        public Task<bool> CreateJobApplication(JobApplicationDto application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application), "Application cannot be null");
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
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<IEnumerable<JobPostSearchDto>> GetAllJobsAsync()
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

        public Task GetJobByIdAsync(int jobId)
        {
            return _context.JobPosts
                .Where(j => j.Id == jobId)
                .FirstOrDefaultAsync();
        }

        public Task<IEnumerable<JobPostSearchDto>> GetJobsByCompanyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JobPostSearchDto>> GetJobsByTitleAsync(string title, string location)
        {
            var jobs = await _context.JobPosts
                .Where(j => j.Title.Contains(title) && j.Location.Contains(location))
                .Select(j => new JobPostSearchDto
                {
                    Title = j.Title,
                    PostedAt = j.PostedAt,
                    CompanyName = j.Company.CompanyName,
                    Location = j.Location
                })
                .ToListAsync();

            return jobs;
        }

      

        
    }
}
