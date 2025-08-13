using JobHub.Models;

namespace JobHub.Interfaces.RepositoriesInterfaces
{
    public interface IProfileReposotity
    {
        public Task<bool> CreateEndUserAsync (EndUser endUser);
        public Task<EndUser?> GetEndUserByIdAsync(string userId);

        public  Task<bool> UpdateEndUserAsync(EndUser endUser);
     
    }
}
