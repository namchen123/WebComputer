using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebComputer.Models;

namespace WebComputer.Controllers
{
    public class ManagementController : Controller
    {
        private readonly ComputerStoreContext _storeContext;
        public ManagementController(ComputerStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public IActionResult ProductManagement()
        {
            var product = _storeContext.Products.Include(p=>p.Suppliers).ToList();
            return View(product);
        }

        public IActionResult CreateProduct()
        {
            var category = _storeContext.Categories.Select(p => new {p.CategoryId, p.CategoryName}).ToList();
            ViewBag.category = new SelectList(category,"CategoryId","CategoryName");
            var supplier =_storeContext.Suppliers.Select(p=> new {p.SupplierId, p.SupplierName}).ToList();
            ViewBag.supplier = new SelectList(supplier, "SupplierId", "SupplierName");
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(NewProduct newProduct)
        {
            Product product = new Product
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Image1 = newProduct.Image1,
                Image2 = newProduct.Image2,
                Image3 = newProduct.Image3,
                Price = newProduct.Price,
                StockQuantity = newProduct.StockQuantity,
                CategoryId = newProduct.CategoryId,
            };
            var supplier = _storeContext.Suppliers.Find(newProduct.SupplierId);
            product.Suppliers.Add(supplier);
            _storeContext.Add(product);
            _storeContext.SaveChanges();

            return RedirectToAction("ProductManagement");
        }
    }
}
