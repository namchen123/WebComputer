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
            var product =_storeContext.Products.Where(p => p.CategoryId == categoryId).Take(5);
            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p=>p.CategoryName).FirstOrDefault();
            ViewBag.categoryname = categoryname;
            ViewBag.categoryId = categoryId;
            return View(product);
        }

        public IActionResult ListProduct2(int categoryId, int categoryId2)
        {
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).Take(5);
            var product2 = _storeContext.Products.Where(p => p.CategoryId == categoryId2).Take(5);
            ViewBag.product = product;
            ViewBag.product2 = product2;

            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname = categoryname;
            var categoryname2 = _storeContext.Categories.Where(p => p.CategoryId == categoryId2).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname2 = categoryname2;

            var categoryid = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid = categoryid;
            var categoryid2 = _storeContext.Categories.Where(p => p.CategoryId == categoryId2).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid2 = categoryid2;
            return View("ListProduct2");
        }
        public IActionResult ListProduct3(int categoryId, int categoryId2, int categoryId3)
        {
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname = categoryname;
            return View(product);
        }
        public IActionResult ListProductByPrice(int categoryId, String description)
        {
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            if (description.Equals("desc"))
            {
                product = product.OrderByDescending(p=>p.Price).ToList();
                var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
                ViewBag.categoryname = categoryname;
                ViewBag.categoryId = categoryId;
            }
            if (description.Equals("asc"))
            {
                product = product.OrderBy(p=>p.Price).ToList();
                var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
                ViewBag.categoryname = categoryname;
                ViewBag.categoryId = categoryId;
            }
            return View(product);
        }

        public IActionResult GetMoreProduct(int pagenumber, int categoryId)
        {
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).Skip((pagenumber - 1)*5).Take(5);
            return PartialView("MoreProduct",product);
        }

    }
}
