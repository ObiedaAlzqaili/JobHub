using JobHub.Models;

namespace JobHub.Interfaces.RepositoriesInterfaces
{
    public interface IProfileReposotity
    {
        public Task<EndUser?> GetEndUserByIdAsync(string userId);

        public  Task<bool> UpdateEndUserAsync(EndUser endUser);
     
    }
}
