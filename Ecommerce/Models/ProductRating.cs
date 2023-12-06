using System;

namespace Ecommerce.Models
{
    public class ProductRating
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}