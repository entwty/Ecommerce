using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Seller
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }

        // Diğer özellikler

        // Satıcı ile ilişkili kullanıcı
        public User User { get; set; }

        // Satıcının ürünleri
        public List<Product> Products { get; set; }

    }
}
