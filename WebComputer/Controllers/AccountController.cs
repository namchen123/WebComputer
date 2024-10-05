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
                TempData["Message"] = "Invalid username or password";
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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) {
                TempData["Message"] = "Please re-enter";
                return View();
            }
            if(model.Password != model.Password2)
            {
                TempData["Message"] = "Incorrect re-enter password";
                return View();
            }
            var account = _storeContext.Accounts.Where(p=>p.Email==model.Username);
            if (account.Any()==true)
            {
                TempData["Message"] = "Username was used";
                return View();
            }
            Account newAccount = new Account();
            newAccount.Email = model.Username;
            newAccount.PasswordHash = model.Password;
            newAccount.CreatedDate = DateTime.UtcNow;
            newAccount.Role = "KH";
            _storeContext.Accounts.AddAsync(newAccount);
            _storeContext.SaveChangesAsync();
            TempData["Message"] = "Create account success";
            return RedirectToAction("Login","Account");
        }

        public IActionResult CustomerDetail()
        {
            var customer = _storeContext.Customers.SingleOrDefault(p=>p.Account.Email.Equals(User.Identity.Name));
            return View(customer);
        }
        [HttpPost]
        public IActionResult CustomerDetail(Customer customer)
        {
            
                customer.AccountId = _storeContext.Accounts.SingleOrDefault(p => p.Email.Equals(User.Identity.Name)).AccountId;
                _storeContext.Customers.Add(customer);
                _storeContext.SaveChanges();
            return View(customer);
        }
    }
    
}
