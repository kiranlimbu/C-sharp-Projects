using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace bugspotAPI.Repositories
{
    public interface IFileService
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        string ConvertByteArrayToFile(byte[] fileData, string extension);
        string GetFileIcon(string file);
        string FormatFileSize(long bytes);
    }
}