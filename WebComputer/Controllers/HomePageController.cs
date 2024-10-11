using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebComputer.Models;

namespace WebComputer.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ComputerStoreContext _storeContext;
        public HomePageController(ComputerStoreContext computerStoreContext)
        {
            _storeContext = computerStoreContext;
        }
        public IActionResult Index()
        {
            ViewBag.cartitemcount = 0;
            if(User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c=>c.Carts).SelectMany(d=>d.CartItems).Count();
            }
            var laptop = _storeContext.Products.Where(p=>p.CategoryId==1).Take(5);
            var console = _storeContext.Products.Where(p => p.CategoryId == 17 || p.CategoryId == 16).OrderBy(p => Guid.NewGuid()).Take(5);
            var keyboard = _storeContext.Products.Where(p=> p.CategoryId == 13).Take(5);
            var mouse = _storeContext.Products.Where(p=>p.CategoryId == 12).Take(5);

            ViewBag.laptop = laptop;
            ViewBag.console = console;
            ViewBag.keyboard = keyboard;
            ViewBag.mouse = mouse;

            return View(laptop);
        }
    }
}
