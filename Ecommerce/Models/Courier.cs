using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Courier
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }

        // Diğer özellikler

        // Kargo gönderileri
        public List<Order> Shipments { get; set; }

        // Kargo göndericisi ile ilişkili kullanıcı
        public User User { get; set; }
    }
}
