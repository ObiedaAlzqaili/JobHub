using System;
using System.Linq;
using System.Threading.Tasks;
using JobHub.Data;
using JobHub.DTOs.Company;
using JobHub.DTOs.Job;
using JobHub.DTOs.UserAccount;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using JobHub.repositories;
using JobHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileReposotity _profileRepository;
        private readonly IOpenAiKeywordExtraction _keywordExtractor;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ApplicationDbContext context, IProfileReposotity profileRepository, IOpenAiKeywordExtraction keywordExtractor,
    ILogger<CompanyController> logger)
        {
            _context = context;
            _profileRepository = profileRepository;
            _keywordExtractor = keywordExtractor;
            _logger = logger;
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
        public async Task<IActionResult> CreateJobPost(JobPostDto model)
        {
            if (ModelState.IsValid)
            {
                var companyId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(companyId))
                {
                    return NotFound();
                }

                var company = await _context.Companies.FindAsync(companyId);

                // Combine relevant fields for AI keyword extraction
                var textToAnalyze = $"{model.Title}\n{model.Description}\n{model.RequiredSkills}\n{model.Location}";

                // Extract AI keywords
                string aiKeywords;
                try
                {
                    aiKeywords = await _keywordExtractor.ExtractKeywordsAsync(textToAnalyze);
                }
                catch (Exception ex)
                {
                    // Log the error but continue with the job post creation
                    // You might want to handle this differently in production
                    _logger.LogError(ex, "Failed to extract AI keywords for job post");
                    aiKeywords = ""; // Set empty or fallback keywords
                }

                var jobPost = new JobPost
                {
                    Title = model.Title,
                    ImageCompanyBase64 = company.PersonalImageBase64,
                    ImageCompanyType = company.PersonalImageType,
                    ImageCompanyName = company.PersonalImageName,
                    companyName = company.CompanyName,
                    Description = model.Description,
                    RequiredSkills = model.RequiredSkills,
                    Location = model.Location,
                    PostedAt = DateTime.Now,
                    CompanyId = companyId,
                    AiKeyWords = aiKeywords // Add the AI-generated keywords
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

            return PartialView("_GetApplicants", applicants);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyProfile(CompanyDataDto companyProfileDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Handle company logo upload
                if (companyProfileDto.CompanyLogo != null && companyProfileDto.CompanyLogo.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await companyProfileDto.CompanyLogo.CopyToAsync(ms);
                        companyProfileDto.CompanyLogoBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    companyProfileDto.CompanyLogoType = companyProfileDto.CompanyLogo.ContentType;
                    companyProfileDto.CompanyLogoName = companyProfileDto.CompanyLogo.FileName;
                }

                // Handle personal image upload
                if (companyProfileDto.PersonalImage != null && companyProfileDto.PersonalImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await companyProfileDto.PersonalImage.CopyToAsync(ms);
                        companyProfileDto.PersonalImageBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    companyProfileDto.PersonalImageType = companyProfileDto.PersonalImage.ContentType;
                    companyProfileDto.PersonalImageName = companyProfileDto.PersonalImage.FileName;
                }

                var companyProfile = new Company
                {
                    Id = userId,
                    FullName = companyProfileDto.FullName,
                    PhoneNumber = companyProfileDto.PhoneNumber,
                    Address = companyProfileDto.Address,
                    DayOfBirth = companyProfileDto.DayOfBirth,
                    Description = companyProfileDto.Description,
                    PersonalImageBase64 = companyProfileDto.PersonalImageBase64,
                    PersonalImageName = companyProfileDto.PersonalImageName,
                    PersonalImageType = companyProfileDto.PersonalImageType,
                    CompanyName = companyProfileDto.FullName,
                    CompanyDescription = companyProfileDto.CompanyDescription,
        
                    JobPosts = new List<JobPost>() // Initialize empty job posts list
                };

                await _profileRepository.CreateCompanyAsync(companyProfile);
                return RedirectToAction("Profile", "Company");
            }

            return View(companyProfileDto);
        }


        [HttpGet]
        public async Task<IActionResult> CreateCompanyProfile()
        {

            var company = new CompanyDataDto();
           

            return View(company);
        }



    }
}