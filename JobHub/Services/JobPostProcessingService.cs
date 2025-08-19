using JobHub.Data;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Models;

namespace JobHub.Services
{
    public class JobPostProcessingService : IJobPostProcessingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOpenAiKeywordExtraction _keywordExtractor;

        public JobPostProcessingService(
            ApplicationDbContext context,
            IOpenAiKeywordExtraction keywordExtractor)
        {
            _context = context;
            _keywordExtractor = keywordExtractor;
        }

        public async Task ProcessJobPostAsync(JobPost jobPost)
        {
            // Combine relevant fields for keyword extraction
            var textToAnalyze = $"{jobPost.Title}\n{jobPost.Description}\n{jobPost.RequiredSkills}";

            // Extract AI keywords
            jobPost.AiKeyWords = await _keywordExtractor.ExtractKeywordsFromJobPostAsync(textToAnalyze);

            // Update the job post
            _context.JobPosts.Update(jobPost);
            await _context.SaveChangesAsync();
        }
    }
}