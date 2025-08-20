using System.Globalization;
using System.Security.Claims;
using JobHub.Data;
using JobHub.DTOs.Ai;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Models;
using JobHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly IResumeProcessingService _resumeProcessingService;
        private readonly IJobMatchingService _jobMatchingService;
        private readonly ApplicationDbContext _context;
        private readonly IOpenAiKeywordExtraction _keywordExtractor;


        public AiController(
            IResumeProcessingService resumeProcessingService,
            IJobMatchingService jobMatchingService,
            ApplicationDbContext context,
            IOpenAiKeywordExtraction keywordExtractor )
        {
            _resumeProcessingService = resumeProcessingService;
            _jobMatchingService = jobMatchingService;
            _context = context;
            _keywordExtractor = keywordExtractor;

        }
        [HttpGet]
        public IActionResult Upload()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingResume = _context.Resumes.FirstOrDefault(r => r.UserId == userId);
            if (existingResume != null)
            {
               
                return RedirectToAction("Results", new { resumeId = existingResume.Id });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Results(int? resumeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the resume - either by ID or the user's most recent resume
            Resume resume;
            if (resumeId.HasValue)
            {
                resume = await _context.Resumes
                    .FirstOrDefaultAsync(r => r.Id == resumeId && r.UserId == userId);
            }
            else
            {
                resume = await _context.Resumes
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.UploadedAt)
                    .FirstOrDefaultAsync();
            }

            if (resume == null)
            {
                return RedirectToAction("Upload");
            }

            // Get matching jobs
            var matchingJobs = await _jobMatchingService.FindMatchingJobsAsync(resume.AiKeywords);

            // Prepare view model
            var viewModel = new ResumeMatchDto
            {
                Resume = resume,
                MatchingJobs = matchingJobs.Select(j => new JobMatchDto
                {
                    JobPost = j,
                    MatchPercentage = CalculateMatchPercentage(resume.AiKeywords, j.AiKeyWords),
                    MatchedSkills = GetMatchedSkills(resume.AiKeywords, j.AiKeyWords)
                }).OrderByDescending(j => j.MatchPercentage).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View();
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                using var stream = file.OpenReadStream();

                // Process the resume and extract keywords
                var resume = await _resumeProcessingService.ProcessUploadedResumeAsync(stream, file.FileName, userId);

                // Get matching jobs
                var matchingJobs = await _jobMatchingService.FindMatchingJobsAsync(resume.AiKeywords);

                // Prepare view model
                var viewModel = new ResumeMatchDto
                {
                    Resume = resume,
                    MatchingJobs = matchingJobs.Select(j => new JobMatchDto
                    {
                        JobPost = j,
                        MatchPercentage = CalculateMatchPercentage(resume.AiKeywords, j.AiKeyWords),
                        MatchedSkills = GetMatchedSkills(resume.AiKeywords, j.AiKeyWords)
                    }).OrderByDescending(j => j.MatchPercentage).ToList()
                };

                return View("Results", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error processing resume: {ex.Message}");
                return View();
            }
        }

        private int CalculateMatchPercentage(string resumeKeywords, string jobKeywords)
        {
            if (string.IsNullOrEmpty(resumeKeywords) || string.IsNullOrEmpty(jobKeywords))
                return 0;

            var resumeSkills = resumeKeywords.Split(',').Select(s => s.Trim().ToLower()).ToList();
            var jobSkills = jobKeywords.Split(',').Select(s => s.Trim().ToLower()).ToList();

            // Weight exact matches higher than partial matches
            var exactMatches = resumeSkills.Count(s => jobSkills.Contains(s));
            var partialMatches = resumeSkills.Count(s => jobSkills.Any(j => j.Contains(s) || s.Contains(j))) - exactMatches;

            var totalScore = (exactMatches * 1.0) + (partialMatches * 0.5);
            var maxPossible = Math.Max(resumeSkills.Count, jobSkills.Count);

            return (int)((totalScore / maxPossible) * 100);
        }

        private List<string> GetMatchedSkills(string resumeKeywords, string jobKeywords)
        {
            if (string.IsNullOrEmpty(resumeKeywords) || string.IsNullOrEmpty(jobKeywords))
                return new List<string>();

            var resumeSkills = resumeKeywords.Split(',').Select(s => s.Trim().ToLower()).ToList();
            var jobSkills = jobKeywords.Split(',').Select(s => s.Trim().ToLower()).ToList();

            return resumeSkills.Intersect(jobSkills).Select(s => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)).ToList();
        }
    }
}
