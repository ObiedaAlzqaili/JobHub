using JobHub.Interfaces.AiInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JobHub.Services.FileHandler
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;
        private readonly string _baseUrl;

        public LocalFileStorageService(IWebHostEnvironment env, IConfiguration configuration)
        {
            // Get storage path from configuration or use default
            _storagePath = configuration["FileStorage:LocalStoragePath"]
                ?? Path.Combine(env.ContentRootPath, "FileStorage");

            // Ensure directory exists
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            // Base URL for file access
            _baseUrl = configuration["FileStorage:BaseUrl"]
                ?? $"{configuration["BaseUrl"]}/files";
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            // Generate unique filename to prevent collisions
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
            var filePath = Path.Combine(_storagePath, uniqueFileName);

            using (var file = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(file);
            }

            return uniqueFileName;
        }

        public Task<Stream> GetFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_storagePath, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found at path: {filePath}");
            }

            Stream stream = new FileStream(fullPath, FileMode.Open);
            return Task.FromResult(stream);
        }

        public Task DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_storagePath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }

        public string GetFileUrl(string filePath)
        {
            return $"{_baseUrl}/{filePath}";
        }
    }
}