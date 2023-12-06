using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string SaveSellerImage(int sellerId, IFormFile image)
        {
            var sellerFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "sellers", sellerId.ToString());

            if (!Directory.Exists(sellerFolder))
            {
                Directory.CreateDirectory(sellerFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(sellerFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public List<string> GetSellerImages(int sellerId)
        {
            var sellerFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "sellers", sellerId.ToString());

            if (!Directory.Exists(sellerFolder))
            {
                return new List<string>();
            }

            var imageFiles = Directory.GetFiles(sellerFolder);
            var imageNames = imageFiles.Select(Path.GetFileName).ToList();

            return imageNames;
        }
    }
}
