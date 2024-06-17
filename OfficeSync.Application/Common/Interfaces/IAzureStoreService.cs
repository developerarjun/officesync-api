namespace OfficeSync.Application.Common.Interfaces
{
    public interface IAzureStoreService
    {
        Task<string> SaveFileAsync(string fileName, byte[] bytes, string folderName);
        Task<byte[]> GetFileAsync(string fileUrl);
        Task DeleteFileAsync(string fileUrl);
    }
}
