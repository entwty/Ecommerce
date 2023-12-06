using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        [NotMapped]
        public string[] ImageFileNames { get; set; } = new string[9]; // Maksimum 9 resim


        private List<ProductImage> _images = new List<ProductImage>();
        public List<ProductImage> Images
        {
            get => _images.Take(9).ToList(); // Maksimum 9 resim
            set => _images = value;
        }


        public Guid SellerId { get; set; }

        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductRating> Ratings { get; set; } = new List<ProductRating>();

       

        public void AddRating(int rating, string comment, string userId)
        {
            var productRating = new ProductRating
            {
                Rating = rating,
                Comment = comment,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            Ratings.Add(productRating);
        }
    }
}
