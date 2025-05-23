﻿using Microsoft.AspNetCore.Mvc;
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
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product = _storeContext.Products.SingleOrDefault(x => x.ProductId == productId);
            var relativeproduct = _storeContext.Products.Where(p => p.CategoryId == product.CategoryId).ToList();
            var specification = _storeContext.ProductSpecifications.Where(p=>p.ProductId == productId).Include(p=>p.Specification);
            ViewBag.specification = specification.Take(4);
            ViewBag.product = product;
            ViewBag.relativeproduct = relativeproduct.Take(5);
            return View(specification);
        }

        public IActionResult ListProduct(int categoryId)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product =_storeContext.Products.Where(p => p.CategoryId == categoryId).Take(5);
            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p=>p.CategoryName).FirstOrDefault();
            var supplier = _storeContext.Suppliers.Where(p=>p.Products.Any(p=>p.CategoryId==categoryId)).ToList();
            ViewBag.supplier = supplier;
            ViewBag.categoryname = categoryname;
            ViewBag.categoryId = categoryId;
            return View(product);
        }

        public IActionResult ListProduct2(int categoryId, int categoryId2)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
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
            return View();
        }
        public IActionResult ListProduct3(int categoryId, int categoryId2, int categoryId3)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).Take(5);
            var product2 = _storeContext.Products.Where(p => p.CategoryId == categoryId2).Take(5);
            var product3 = _storeContext.Products.Where(p => p.CategoryId == categoryId3).Take(5);
            ViewBag.product = product;
            ViewBag.product2 = product2;
            ViewBag.product3 = product3;

            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname = categoryname;
            var categoryname2 = _storeContext.Categories.Where(p => p.CategoryId == categoryId2).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname2 = categoryname2;
            var categoryname3 = _storeContext.Categories.Where(p => p.CategoryId == categoryId3).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname3 = categoryname3;

            var categoryid = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid = categoryid;
            var categoryid2 = _storeContext.Categories.Where(p => p.CategoryId == categoryId2).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid2 = categoryid2;
            var categoryid3 = _storeContext.Categories.Where(p => p.CategoryId == categoryId3).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid3 = categoryid3;

            return View(product);
        }

        public IActionResult ListProduct9()
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product = _storeContext.Products.Where(p => p.CategoryId == 2).Take(5);
            var product2 = _storeContext.Products.Where(p => p.CategoryId == 3).Take(5);
            var product3 = _storeContext.Products.Where(p => p.CategoryId == 4).Take(5);
            var product4 = _storeContext.Products.Where(p => p.CategoryId == 5).Take(5);
            var product5 = _storeContext.Products.Where(p => p.CategoryId == 6).Take(5);
            var product6 = _storeContext.Products.Where(p => p.CategoryId == 7).Take(5);
            var product7 = _storeContext.Products.Where(p => p.CategoryId == 8).Take(5);
            var product8 = _storeContext.Products.Where(p => p.CategoryId == 9).Take(5);
            var product9 = _storeContext.Products.Where(p => p.CategoryId == 10).Take(5);
            ViewBag.product = product;
            ViewBag.product2 = product2;
            ViewBag.product3 = product3;
            ViewBag.product4 = product4;
            ViewBag.product5 = product5;
            ViewBag.product6 = product6;
            ViewBag.product7 = product7;
            ViewBag.product8 = product8;
            ViewBag.product9 = product9;

            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == 2).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname = categoryname;
            var categoryname2 = _storeContext.Categories.Where(p => p.CategoryId == 3).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname2 = categoryname2;
            var categoryname3 = _storeContext.Categories.Where(p => p.CategoryId == 4).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname3 = categoryname3;
            var categoryname4 = _storeContext.Categories.Where(p => p.CategoryId == 5).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname4 = categoryname4;
            var categoryname5 = _storeContext.Categories.Where(p => p.CategoryId == 6).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname5 = categoryname5;
            var categoryname6 = _storeContext.Categories.Where(p => p.CategoryId == 7).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname6 = categoryname6;
            var categoryname7 = _storeContext.Categories.Where(p => p.CategoryId == 8).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname7 = categoryname7;
            var categoryname8 = _storeContext.Categories.Where(p => p.CategoryId == 9).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname8 = categoryname8;
            var categoryname9 = _storeContext.Categories.Where(p => p.CategoryId == 10).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname9 = categoryname9;

            var categoryid = _storeContext.Categories.Where(p => p.CategoryId == 2).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid = categoryid;
            var categoryid2 = _storeContext.Categories.Where(p => p.CategoryId == 3).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid2 = categoryid2;
            var categoryid3 = _storeContext.Categories.Where(p => p.CategoryId == 4).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid3 = categoryid3;
            var categoryid4 = _storeContext.Categories.Where(p => p.CategoryId == 5).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid4 = categoryid4;
            var categoryid5 = _storeContext.Categories.Where(p => p.CategoryId == 6).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid5 = categoryid5;
            var categoryid6 = _storeContext.Categories.Where(p => p.CategoryId == 7).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid6 = categoryid6;
            var categoryid7 = _storeContext.Categories.Where(p => p.CategoryId == 8).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid7 = categoryid7;
            var categoryid8 = _storeContext.Categories.Where(p => p.CategoryId == 9).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid8 = categoryid8;
            var categoryid9 = _storeContext.Categories.Where(p => p.CategoryId == 10).Select(p => p.CategoryId).FirstOrDefault();
            ViewBag.categoryid9 = categoryid9;

            return View();
        }

        public IActionResult ListProductByPrice(int categoryId, String description)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            if (description.Equals("desc"))
            {
                product = product.OrderByDescending(p=>p.Price).Take(5).ToList();
                var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
                var supplier = _storeContext.Suppliers.Where(p => p.Products.Any(p => p.CategoryId == categoryId)).ToList();
                ViewBag.supplier = supplier;
                ViewBag.categoryname = categoryname;
                ViewBag.categoryId = categoryId;
                ViewBag.orderby = "desc";
            }
            if (description.Equals("asc"))
            {
                product = product.OrderBy(p=>p.Price).Take(5).ToList();
                var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
                var supplier = _storeContext.Suppliers.Where(p => p.Products.Any(p => p.CategoryId == categoryId)).ToList();
                ViewBag.supplier = supplier;
                ViewBag.categoryname = categoryname;
                ViewBag.categoryId = categoryId;
                ViewBag.orderby = "asc";
            }
            return View(product);
        }

        public IActionResult ListProductByPrice2(decimal minprice, decimal maxprice, int categoryId)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == categoryId).Select(p => p.CategoryName).FirstOrDefault();
            var product = _storeContext.Products.Where(p=> p.CategoryId == categoryId && p.Price>=minprice && p.Price<=maxprice).ToList();
            var supplier = _storeContext.Suppliers.Where(p => p.Products.Any(p => p.CategoryId == categoryId)).ToList();
            ViewBag.supplier = supplier;
            ViewBag.categoryname = categoryname;
            ViewBag.categoryId = categoryId;
            return View(product);
        }

        public IActionResult ListProductBySupplier(int supplierId, int categoryId)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product = _storeContext.Products.Where(p=>p.CategoryId==categoryId && p.Suppliers.Any(p=>p.SupplierId==supplierId)).ToList();
            return PartialView("ListProductBySupplier",product);
        }

        public IActionResult GetMoreProduct(int pagenumber, int categoryId, string orderby)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var product = _storeContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            if (orderby.Equals("desc"))
            {
                product = product.OrderByDescending(p => p.Price).Skip((pagenumber - 1) * 5).Take(5).ToList();
            }
            if (orderby.Equals("asc"))
            {
                product = product.OrderBy(p => p.Price).Skip((pagenumber - 1) * 5).Take(5).ToList();
            }
            if (orderby.Equals("noorder"))
            {
                product = product.Skip((pagenumber - 1) * 5).Take(5).ToList();
            }
            return PartialView("MoreProduct",product);
        }
        [HttpGet]
        public IActionResult FindProduct(String name)
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            decimal price = 0;
            bool isNumeric = decimal.TryParse(name, out price);
            var product = _storeContext.Products.Where(p=>p.Name.Contains(name) || p.Category.CategoryName.Contains(name) || (isNumeric && p.Price >= price - 300000 && p.Price <= price + 300000)).ToList();
            ViewBag.name = name;
            return View(product);
        }

        public IActionResult BuildPC()
        {
            ViewBag.cartitemcount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var accountid = _storeContext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
                ViewBag.cartitemcount = _storeContext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            }
            var categoryname = _storeContext.Categories.Where(p => p.CategoryId == 2).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname = categoryname;
            var categoryname2 = _storeContext.Categories.Where(p => p.CategoryId == 3).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname2 = categoryname2;
            var categoryname3 = _storeContext.Categories.Where(p => p.CategoryId == 4).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname3 = categoryname3;
            var categoryname4 = _storeContext.Categories.Where(p => p.CategoryId == 5).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname4 = categoryname4;
            var categoryname5 = _storeContext.Categories.Where(p => p.CategoryId == 6).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname5 = categoryname5;
            var categoryname6 = _storeContext.Categories.Where(p => p.CategoryId == 7).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname6 = categoryname6;
            var categoryname7 = _storeContext.Categories.Where(p => p.CategoryId == 8).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname7 = categoryname7;
            var categoryname8 = _storeContext.Categories.Where(p => p.CategoryId == 9).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname8 = categoryname8;
            var categoryname9 = _storeContext.Categories.Where(p => p.CategoryId == 10).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname9 = categoryname9;
            var categoryname10 = _storeContext.Categories.Where(p => p.CategoryId == 11).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname10 = categoryname10;
            var categoryname11 = _storeContext.Categories.Where(p => p.CategoryId == 12).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname11 = categoryname11;
            var categoryname12 = _storeContext.Categories.Where(p => p.CategoryId == 13).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname12 = categoryname12;
            var categoryname13 = _storeContext.Categories.Where(p => p.CategoryId == 14).Select(p => p.CategoryName).FirstOrDefault();
            ViewBag.categoryname13 = categoryname13;

            ViewBag.categoryid = 2;
            ViewBag.categoryid2 = 3;
            ViewBag.categoryid3 = 4;
            ViewBag.categoryid4 = 5;
            ViewBag.categoryid5 = 6;
            ViewBag.categoryid6 = 7;
            ViewBag.categoryid7 = 8;
            ViewBag.categoryid8 = 9;
            ViewBag.categoryid9 = 10;
            ViewBag.categoryid10 = 11;
            ViewBag.categoryid11 = 12;
            ViewBag.categoryid12 = 13;
            ViewBag.categoryid13 = 14;
            return View();
        }
    }
}
