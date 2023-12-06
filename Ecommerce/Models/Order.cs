using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Order
    {
        public Guid OrderId { get; set; } 
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public bool IsApprovedBySeller { get; set; }
        public bool IsShipped { get; set; }
    }
}
