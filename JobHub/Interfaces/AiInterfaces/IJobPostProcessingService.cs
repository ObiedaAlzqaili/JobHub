using JobHub.Models;

namespace JobHub.Interfaces.AiInterfaces
{
    public interface IJobPostProcessingService
    {
        public Task ProcessJobPostAsync(JobPost jobPost);
    }
}
