using JobHub.Data;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Models;
using System.Linq;

namespace JobHub.Services
{
    public class JobMatchingService : IJobMatchingService
    {
        private readonly ApplicationDbContext _context;

        public JobMatchingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobPost>> FindMatchingJobsAsync(string resumeAiKeywords)
        {
            if (string.IsNullOrEmpty(resumeAiKeywords))
                return new List<JobPost>();

            var keywords = resumeAiKeywords.Split(',')
                .Select(k => k.Trim().ToLower())
                .Where(k => !string.IsNullOrEmpty(k))
                .ToArray();

            // Get jobs with at least 3 matching keywords
            var query = _context.JobPosts
                .Where(j => !string.IsNullOrEmpty(j.AiKeyWords))
                .AsEnumerable() // Switch to client-side evaluation for string operations
                .Where(j => keywords.Count(k => j.AiKeyWords.ToLower().Contains(k)) >= 3)
                .OrderByDescending(j => keywords.Count(k => j.AiKeyWords.ToLower().Contains(k)))
                .Take(20);

            return await Task.FromResult(query.ToList());
        }
    }
}