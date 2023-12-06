using System;

namespace Ecommerce.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }

        // Diğer özellikler

        // İlgili ürün
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}