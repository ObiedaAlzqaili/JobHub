using JobHub.Data;
using JobHub.DTOs.Ai;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace JobHub.Controllers
{
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
            return View();
        }

        [HttpGet]
        public IActionResult Results()
        {
            return View();
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

            var matchedCount = resumeSkills.Count(s => jobSkills.Contains(s));
            var totalPossible = Math.Max(resumeSkills.Count, jobSkills.Count);

            return (int)((matchedCount / (double)totalPossible) * 100);
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
