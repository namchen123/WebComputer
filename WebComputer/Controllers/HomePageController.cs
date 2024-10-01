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
            var product = _storeContext.Products.ToList();
            return View(product);
        }
    }
}
