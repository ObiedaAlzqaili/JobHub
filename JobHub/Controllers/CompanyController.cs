using JobHub.Data;
using JobHub.DTOs.Job;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobHub.Controllers
{
    [Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Profile()
        {
            var companyId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyId))
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public async Task<IActionResult> JobPosts()
        {
            var companyId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyId))
            {
                return NotFound();
            }

            var jobPosts = await _context.JobPosts
                .Where(j => j.CompanyId == companyId)
                .Include(j => j.JobApplications)
                .ToListAsync();

            return View(jobPosts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJobPost(JobPost model)
        {
            if (ModelState.IsValid)
            {
                var companyId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(companyId))
                {
                    return NotFound();
                }

                var jobPost = new JobPost
                {
                    Title = model.Title,
                    Description = model.Description,
                    RequiredSkills = model.RequiredSkills,
                    Location = model.Location,
                    PostedAt = DateTime.Now,
                    CompanyId = companyId
                };

                _context.JobPosts.Add(jobPost);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(JobPosts));
            }

            return PartialView("_CreateJobPostModal", model);
        }

        public async Task<IActionResult> GetApplicants(int jobPostId)
        {
            var applicants = await _context.JobApplications
                .Where(a => a.JobPostId == jobPostId)
                .Select(a => new JobApplicationDto
                {
                    ApplicantName = a.Name,
                    ApplicantEmail = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    JobTitle = a.JobPost.Title,
                    CompanyName = a.JobPost.companyName,
                    ResumeBase64 = a.ResumeBase64,
                    ResumeFileName = a.ResumeName,
                    ResumeFileType = a.ResumeType,
                    ApplicationDate = a.AppliedOn,
                    JobPostId = a.JobPostId,
                    EndUserId = a.EndUserId
                })
                .ToListAsync();

            ViewBag.JobTitle = await _context.JobPosts
                .Where(j => j.Id == jobPostId)
                .Select(j => j.Title)
                .FirstOrDefaultAsync();

            return PartialView("_ApplicantsModal", applicants);
        }
    }
}