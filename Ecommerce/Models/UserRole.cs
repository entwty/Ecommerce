using System;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; } // 'virtual' anahtar kelimesi, 'User' sınıfınızı temsil eder

        public virtual Role Role { get; set; } // 'virtual' anahtar kelimesi, 'Role' sınıfınızı temsil eder
    }
}