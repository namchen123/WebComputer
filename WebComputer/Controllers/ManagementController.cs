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
        public IActionResult CustomerManagement()
        {
            var customer = _storeContext.Customers.Where(p => p.Account.Role.Equals("KH")).Include(p=>p.Account);
            return View(customer);
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

        public IActionResult AddSpecification(int productId)
        {
            var product = _storeContext.Products.Find(productId);
            ViewBag.product = product;
            var productSpecification = _storeContext.Specifications.Select(p=> new {p.SpecificationId, p.SpecificationName}).ToList();
            ViewBag.productSpecification = new SelectList(productSpecification, "SpecificationId", "SpecificationName");
            return View();
        }
        public IActionResult AddSpecificationSuccess(ProductSpecification productSpecification)
        {
            if (_storeContext.ProductSpecifications.Where(p => p.ProductId == productSpecification.ProductId && p.SpecificationId == productSpecification.SpecificationId).Any())
            {
                TempData["Message"] = "Specification is already added";
                return RedirectToAction("AddSpecification", new { productId = productSpecification.ProductId });
            };
            _storeContext.Add(productSpecification);
            _storeContext.SaveChanges();
            return RedirectToAction("AddSpecification", new { productId = productSpecification.ProductId });
        }

        public IActionResult EditProduct(int productId)
        {
            var category = _storeContext.Categories.Select(p => new { p.CategoryId, p.CategoryName }).ToList();
            ViewBag.category = new SelectList(category, "CategoryId", "CategoryName");
            var product = _storeContext.Products.Find(productId);
            return View(product);
        }
        public IActionResult EditProductSuccess(Product product)
        {
            _storeContext.Products.Update(product);
            _storeContext.SaveChanges();
            TempData["Message"] = "Update success!";
            return RedirectToAction("ProductManagement");
        }

        public IActionResult DeleteProduct(int productId)
        {
            var product = _storeContext.Products.Include(p=>p.Category).SingleOrDefault(p=>p.ProductId==productId);
            return View(product);
        }

        public IActionResult DeleteProductSuccess(int ProductId)
        {
            var product = _storeContext.Products.Find(ProductId);
            _storeContext.Remove(product);
            _storeContext.SaveChanges();
            TempData["Message"] = "Delete success!";
            return RedirectToAction("ProductManagement");
        }
    }
}
