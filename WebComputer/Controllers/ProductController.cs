using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebComputer.Models;

namespace WebComputer.Controllers
{
    public class ProductController : Controller
    {
        private readonly ComputerStoreContext _storeContext;
        public ProductController(ComputerStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public IActionResult ProductDetail(int productId)
        {
            var product = _storeContext.Products.Where(x => x.ProductId == productId).ToList();
            var specification = _storeContext.ProductSpecifications.Where(p=>p.ProductId == productId).Include(p=>p.Specification);
            ViewBag.specification = specification.Take(4);
            ViewBag.product = product;
            return View(specification);
        }

        public IActionResult ListProduct(int categoryId)
        {
            var product =_storeContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            return View(product);
        }
    }
}
