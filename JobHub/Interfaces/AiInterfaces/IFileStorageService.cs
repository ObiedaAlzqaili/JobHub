

namespace JobHub.Interfaces.AiInterfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName);
        Task<Stream> GetFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
        string GetFileUrl(string filePath);
    }
}
