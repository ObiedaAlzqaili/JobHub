using JobHub.DTOs;
using JobHub.DTOs.Job;

namespace JobHub.Interfaces.RepositoriesInterfaces
{
    public interface IJobRepository
    {

        public Task  GetJobByIdAsync(int jobId);

        public Task<IEnumerable<JobPostSearchDto>> GetFiveJobAsync();

        public Task<IEnumerable<JobPostSearchDto>> GetAllJobsAsync();

        public Task<IEnumerable<JobPostSearchDto>> GetJobsByTitleAsync(string title , string location);
        
        public Task<IEnumerable<JobPostSearchDto>> GetJobsByCompanyAsync(int id);

        public Task<bool> CreateJobApplication(JobApplicationDto application);

    }
}
