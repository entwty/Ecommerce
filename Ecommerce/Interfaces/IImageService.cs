using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Interfaces
{
    public interface IImageService
    {
        string SaveSellerImage(int sellerId, IFormFile image);
        List<string> GetSellerImages(int sellerId);
    }
}
