using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using OfficeSync.Application.Common.Interfaces;

namespace OfficeSync.Infrastructure.Services
{
    public class AzureStoreService : IAzureStoreService
    {
        private readonly BlobContainerClient _blobContainerClient;
        public AzureStoreService(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<string> SaveFileAsync(string fileName, byte[] bytes, string folderName)
        {
            await _blobContainerClient.CreateIfNotExistsAsync();

            var stream = new MemoryStream(bytes);

            // Upload file
            await _blobContainerClient.UploadBlobAsync($"{folderName}/{fileName}", stream);

            var resut = $"{_blobContainerClient.Uri}/{folderName}/{fileName}";
            return resut;
        }

        public async Task<byte[]> GetFileAsync(string fileUrl)
        {
            var path = _blobContainerClient.Uri.AbsoluteUri;
            var fileName = fileUrl.Replace(path, "");
            var response = _blobContainerClient.GetBlockBlobClient(fileName);

            var ms = new MemoryStream();
            await response.DownloadToAsync(ms);

            var bytes = ms.ToArray();

            return bytes;
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            await _blobContainerClient.DeleteBlobIfExistsAsync(fileUrl);
        }
    }
}
