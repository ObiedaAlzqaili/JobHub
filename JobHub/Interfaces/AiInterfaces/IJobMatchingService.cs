using JobHub.Models;

namespace JobHub.Interfaces.AiInterfaces
{
    public interface IJobMatchingService
    {
        public Task<List<JobPost>> FindMatchingJobsAsync(string resumeAiKeywords);
    }
}
