using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebComputer.Models;

namespace WebComputer.Controllers
{
    public class AccountController : Controller
    {
        private readonly ComputerStoreContext _storeContext;
        public AccountController(ComputerStoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var account = _storeContext.Accounts.FirstOrDefault(p=>p.Email == model.Username);
            if (account == null || account.PasswordHash != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model);
            }
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.Email),
            new Claim("FullName", account.Email),
            new Claim(ClaimTypes.Role, account.Role),
            new Claim(ClaimTypes.Sid, account.AccountId.ToString())
        };

            var claimsIdentity = new ClaimsIdentity(claims, "Login");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "HomePage");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "HomePage");
        }
    }
    
}
