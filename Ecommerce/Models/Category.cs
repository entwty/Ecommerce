using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; } 

        
        public string Name { get; set; }

        public bool IsApprovedByAdmin { get; set; }

        public List<Product> Products { get; set; }
    }
}
