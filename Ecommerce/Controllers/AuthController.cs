using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.DataTransferObjects;
using Ecommerce.Interfaces;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IFileService _fileService;
        private readonly IJwtService _jwtService; // Eğer JWT kullanacaksanız

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IFileService fileService,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _fileService = fileService;
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                IsApproved = true, // Müşteri kaydı otomatik olarak onaylanmış olarak kabul ediliyor
            };


            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateJwtToken(user, roles);

                return Ok(new { UserId = user.Id, Token = token, IsApproved = user.IsApproved });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login/admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.IsInRoleAsync(user, "ADMIN"))
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Admin logged in successfully." });
                }
            }

            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        [HttpPost("/register/seller")]
        public async Task<IActionResult> RegisterSeller([FromForm] SellerRegistrationModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                IsApproved = false, // Satıcı kaydı otomatik olarak onaylanmamış olarak kabul ediliyor
                BankAccountNumber = model.BankAccountNumber,
               
            };

            if (model.CompanyDocument != null && model.CompanyDocument.Length > 0)
            {
                user.CompanyDocumentFileName = await _fileService.UploadFileAsync(
                    model.CompanyDocument, "sellerFiles", user.Id.ToString()
                );
            }

            if (model.TaxCertificate != null && model.TaxCertificate.Length > 0)
            {
                user.TaxCertificateFileName = await _fileService.UploadFileAsync(
                    model.TaxCertificate, "sellerFiles", user.Id.ToString()
                );
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "SELLER");

                // Satıcı kaydının onay sürecini başlatmak için gerekirse ek işlemler yapabilirsiniz

                // Dosya adlarını kontrol et
                if (!string.IsNullOrEmpty(user.CompanyDocumentFileName) && !string.IsNullOrEmpty(user.TaxCertificateFileName))
                {
                    return Ok(new { UserId = user.Id, IsApproved = user.IsApproved });
                }
                else
                {
                    // Eğer dosya adları eksikse, bir hata durumunu belirleyebilirsiniz
                    return BadRequest("Dosya yükleme hatası");
                }
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("register/courier")]
        public async Task<IActionResult> RegisterCourier([FromBody] CourierRegistrationModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                IsApproved = false, // Kargocu kaydı otomatik olarak onaylanmamış olarak kabul ediliyor
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Courier");

                // Kargocu kaydının onay sürecini başlatmak için gerekirse ek işlemler yapabilirsiniz

                return Ok(new { UserId = user.Id, IsApproved = user.IsApproved });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login/customer")]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Customer logged in successfully." });
            }

            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        [HttpPost("login/seller")]
        public async Task<IActionResult> LoginSeller([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.IsInRoleAsync(user, "Seller"))
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (user.IsApproved)
                    {
                        return Ok(new { Message = "Seller logged in successfully." });
                    }
                    else
                    {
                        return Unauthorized(new { Message = "Seller is not approved. Waiting for approval." });
                    }
                }
            }

            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        [HttpPost("login/courier")]
        public async Task<IActionResult> LoginCourier([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.IsInRoleAsync(user, "Courier"))
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (user.IsApproved)
                    {
                        return Ok(new { Message = "Courier logged in successfully." });
                    }
                    else
                    {
                        return Unauthorized(new { Message = "Courier is not approved. Waiting for approval." });
                    }
                }
            }

            return Unauthorized(new { Message = "Invalid login attempt." });
        }




        [HttpGet("check-approval/{userId}")]
        public async Task<IActionResult> CheckApproval(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            if (user.IsApproved)
            {
                return Ok(new { Message = "User is approved." });
            }
            else
            {
                return Ok(new { Message = "User is not approved. Waiting for approval." });
            }
        }

    }
}
