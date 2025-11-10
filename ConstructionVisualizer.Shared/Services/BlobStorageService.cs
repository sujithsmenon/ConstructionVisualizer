using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Shared
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _blobServiceClient = new BlobServiceClient(_configuration["AzureStorage:ConnectionString"]);
        }

        public async Task<string> UploadImageAsync(IFormFile image, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = image.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }

        public async Task<string> GenerateThumbnailAsync(string imageUrl, int width, int height)
        {
            // Implementation using ImageSharp or similar library
            // This would resize the image and upload thumbnail
            return await Task.FromResult(imageUrl + "?thumb=true");
        }
    }

    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(IFormFile image, string containerName);
        Task<string> GenerateThumbnailAsync(string imageUrl, int width, int height);
    }
}
