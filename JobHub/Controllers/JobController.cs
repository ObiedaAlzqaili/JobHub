using DocumentFormat.OpenXml.EMMA;
using JobHub.Data;
using JobHub.DTOs.Job;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using JobHub.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobRepository _jobRepository;
        private readonly ApplicationDbContext _context;

        public JobController(IJobRepository jobRepository , ApplicationDbContext context)
        {
            _jobRepository = jobRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var jobposts = await _jobRepository.GetFiveJobAsync();
            return View(jobposts);
        }
        [HttpGet]
        public IActionResult JobSearchResult(string title, string location)
        {
            var jobPosts = _jobRepository.GetJobsByTitleAsync(title, location).Result;
            return View(jobPosts);
        }

        [HttpGet]
        public async Task<IActionResult> JobPostDetails(int Id)
        {
            var jobPost = await _jobRepository.GetJobByIdAsync(Id);

            return View(jobPost);
        }

        [HttpGet]
        public async Task<IActionResult> JobPostApplication(int Id)
        {
            var jobPost =  _jobRepository.GetJobByIdAsync(Id);
            var jobApplication = new JobApplicationDto
            {
                JobPostId = Id,
                ApplicationDate = DateTime.UtcNow,
                ApplicantName = string.Empty, // Placeholder, to be filled by the user                                                  
                ApplicantEmail = string.Empty, 
                PhoneNumber = string.Empty,                              
                ResumeBase64 = string.Empty,
                ResumeFileName = string.Empty, 
                ResumeFileType = string.Empty,
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JobPostApplication(string FullName, string Email, string Phone, IFormFile Resume, int companyId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not authenticated
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Check if user has already applied for this job
            var existingApplication = await _context.JobApplications
                .FirstOrDefaultAsync(ja => ja.JobPostId == companyId && ja.EndUserId == userId);

            if (existingApplication != null)
            {
                TempData["ErrorMessage"] = "You have already applied for this position.";
                return RedirectToAction("JobPostDetails", new { id = companyId });
            }

            if (ModelState.IsValid)
            {
                string resumeBase64 = null;
                string resumeType = null;
                string resumeName = null;

                if (Resume != null && Resume.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await Resume.CopyToAsync(ms);
                        resumeBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    resumeType = Resume.ContentType;
                    resumeName = Resume.FileName;
                }

                var jobApplication = new JobApplicationDto
                {
                    JobPostId = companyId,
                    EndUserId = userId,
                    ApplicantName = FullName,
                    ApplicantEmail = Email,
                    PhoneNumber = Phone,
                    ApplicationDate = DateTime.UtcNow,
                    ResumeBase64 = resumeBase64,
                    ResumeFileName = resumeName,
                    ResumeFileType = resumeType
                };

                var result = await _jobRepository.CreateJobApplication(jobApplication);

                if (result)
                {
                    TempData["SuccessMessage"] = "Application submitted successfully!";
                    return RedirectToAction("JobPostDetails", new { id = companyId });
                }
                else
                {
                    ModelState.AddModelError("", "Failed to submit application. Please try again.");
                }
            }

            // If we got this far, something failed; redisplay form
            return RedirectToAction("JobPostDetails", new { id = companyId });
        }


    }
}
