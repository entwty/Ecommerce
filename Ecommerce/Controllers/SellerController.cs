using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/sellers")]
    public class SellerController : ControllerBase
    {

        private readonly IImageService _imageService;

        public SellerController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("{sellerId}/uploadImage")]
        public IActionResult UploadImage(int sellerId, IFormFile image)
        {
            var savedFileName = _imageService.SaveSellerImage(sellerId, image);

            // Resim yüklendiyse, işleme devam edebilirsiniz.

            return Ok(new { ImageFileName = savedFileName });
        }

        [HttpGet("{sellerId}/images")]
        public IActionResult GetSellerImages(int sellerId)
        {
            var images = _imageService.GetSellerImages(sellerId);

            return Ok(images);
        }
    }
}
