using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<UserRole> UserRoles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }

        // Sadece satıcılar ve kargocular için geçerli olan özellikler
        public string CompanyDocumentFileName { get; set; } // Şirket belgesi dosya adı için
        public string TaxCertificateFileName { get; set; } // Vergi levhası dosya adı için
        public string BankAccountNumber { get; set; } // Banka numarası için

        // IdentityUser sınıfındaki diğer özellikler
        public override string Email { get; set; }
        public override string UserName { get => Email; set => Email = value; }
    }
}
