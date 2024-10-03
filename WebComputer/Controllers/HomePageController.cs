using Microsoft.AspNetCore.Mvc;
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
            var laptop = _storeContext.Products.Where(p=>p.CategoryId==1).Take(5);
            var console = _storeContext.Products.Where(p => p.CategoryId == 17 || p.CategoryId == 16).Take(5);
            ViewBag.laptop = laptop;
            ViewBag.console = console;
            return View(laptop);
        }
    }
}
