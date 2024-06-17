using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace OfficeSync.Application.Common.Helpers
{
    public static class FileHelper
    {
        private readonly static string _rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static async Task<string> ReadEmailTemplateAsync(string templateName, CancellationToken cancellationToken)
        {
            //Your template file should be in the debug/release folder. 
            string filePath = Path.Combine(_rootPath, $"Common\\EmailTemplates\\{templateName}");
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            string html = string.Empty;
            using (var sr = new StreamReader(filePath))
            {
                html = await sr.ReadToEndAsync();
            }

            return html;
        }

        public static async Task<byte[]> GetFileBytesAsync(this IFormFile formFile)
        {
            using var mstream = new MemoryStream();
            await formFile.CopyToAsync(mstream);
            var fileBytes = mstream.ToArray();

            return fileBytes;
        }

        public static async Task<Stream> GetFileStreamAsync(this IFormFile formFile)
        {
            var fileBytes = await formFile.GetFileBytesAsync();
            var stream = new MemoryStream(fileBytes);

            return stream;
        }

        public static string GetExtension(this IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension))
            {
                var contentType = file.ContentType;
                extension = $".{contentType.Split('/')[1]}";
            }
            return extension;
        }
    }
}
