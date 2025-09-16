using Microsoft.AspNetCore.Http;

namespace Agendamento.Infra.CrossCutting.Util.Extensions
{
    public static class FileExtensions
    {
        public static async Task<string> ToBase64StringAsync(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }

        public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}