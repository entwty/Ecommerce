using Microsoft.AspNetCore.Http;

namespace Ecommerce.DataTransferObjects
{
    public class SellerRegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public IFormFile CompanyDocument { get; set; } // Şirket belgeleri, vb. için
        public IFormFile TaxCertificate { get; set; } // Vergi levhası için
        public string BankAccountNumber { get; set; } // Banka numarası için
    }
}