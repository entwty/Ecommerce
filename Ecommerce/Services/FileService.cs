using System;
using System.IO;
using System.Threading.Tasks;
using Ecommerce.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Services
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string folderName, string userId)
        {
            // Satıcıya özel klasörü oluştur
            var userFolder = Path.Combine("wwwroot", folderName, userId);

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            // Dosyayı kaydet
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(userFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }

}