using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, string userId);
    }
}