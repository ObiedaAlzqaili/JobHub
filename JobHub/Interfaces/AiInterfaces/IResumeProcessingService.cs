using JobHub.Models;

namespace JobHub.Interfaces.AiInterfaces
{
    public interface IResumeProcessingService
    {
        public  Task<Resume> ProcessUploadedResumeAsync(Stream fileStream, string fileName, string userId);
    }
}
