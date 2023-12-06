using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user, IList<string> roles);
        Task<bool> ValidateJwtToken(string token);
    }
}