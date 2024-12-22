using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
            var totalproduct = _storeContext.Products.Count();
            int productperpage = 15;
            int pagenumber = (int)Math.Ceiling((double)totalproduct / productperpage);
            ViewBag.pagenumber = Enumerable.Range(0, pagenumber).ToList();
            var product = _storeContext.Products.Include(p=>p.Suppliers).ToList();
            return View(product);
        }
        public IActionResult ProductPagination(int page)
        {
            var product = _storeContext.Products.Include(p=>p.Suppliers).Skip((page-1)*15).Take(15).ToList();
            return PartialView("ProductPagination",product);
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
            var category = _storeContext.Categories.Select(p => new { p.CategoryId, p.CategoryName }).ToList();
            ViewBag.category = new SelectList(category, "CategoryId", "CategoryName");
            var esupplier = _storeContext.Suppliers.Select(p => new { p.SupplierId, p.SupplierName }).ToList();
            ViewBag.supplier = new SelectList(esupplier, "SupplierId", "SupplierName");
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (newProduct.StockQuantity < 0)
            {
                return View();
            }
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

        public IActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existaccount = _storeContext.Accounts.SingleOrDefault(p => p.Email.Equals(customer.Account.Email));
            if (existaccount != null)
            {
                TempData["Message"] = "Account name already exist";
                return View();
            }
            var account = new Account
            {
                Email = customer.Account.Email,
                PasswordHash = customer.Account.PasswordHash,
                Role = customer.Account.Role,
            };
            _storeContext.Accounts.Add(account);
            _storeContext.SaveChanges();

            var newcustomer = new Customer
            {
                AccountId = account.AccountId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Address = customer.Address,
            };
            _storeContext.Customers.Add(newcustomer);
            _storeContext.SaveChanges();

            Cart cart = new Cart { CustomerId = newcustomer.CustomerId };

            _storeContext.Carts.Add(cart);
            _storeContext.SaveChanges();

            TempData["Message"] = "Add user success";
            return RedirectToAction("CustomerManagement");
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

        public IActionResult EditCustomer(int customerId)
        {
            var customer = _storeContext.Customers.Include(p=>p.Account).SingleOrDefault(p=>p.CustomerId==customerId);
            return View(customer);
        }

        public IActionResult EditCustomerSuccess(Customer customer)
        {

            var existingCustomer = _storeContext.Customers
            .Include(c => c.Account)
            .SingleOrDefault(c => c.CustomerId == customer.CustomerId);

            if (existingCustomer != null)
            {
                // Cập nhật các thuộc tính
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Address = customer.Address;

                // Cập nhật thuộc tính của Account
                existingCustomer.Account.Email = customer.Account.Email;
                existingCustomer.Account.PasswordHash = customer.Account.PasswordHash;
                existingCustomer.Account.Role = customer.Account.Role;

                // Lưu lại
                _storeContext.SaveChanges();
                TempData["Message"] = "Update success!";
            }
            return RedirectToAction("CustomerManagement");
        }
        public IActionResult DeleteCustomer(int customerId)
        {
            var customer = _storeContext.Customers.Include(p=>p.Account).SingleOrDefault(p=>p.CustomerId==customerId);
            return View(customer);
        }

        public IActionResult DeleteCustomerSuccess(int CustomerId)
        {
            var customer = _storeContext.Customers.SingleOrDefault(p=>p.CustomerId==CustomerId);
            var account = _storeContext.Accounts.SingleOrDefault(p => p.AccountId == customer.AccountId);
            _storeContext.Remove(customer);
            _storeContext.Remove(account);
            _storeContext.SaveChanges();
            return RedirectToAction("CustomerManagement");
        }

        public IActionResult AdvertisementManagement()
        {
            var advertisement = _storeContext.Advertisements.Include(p=>p.Product).ToList();
            return View(advertisement);
        }

        public IActionResult CreateAdvertisement()
        {
            var product = _storeContext.Products.Select(p => new { p.ProductId, p.Name });
            ViewBag.product = new SelectList(product,"ProductId","Name");
            return View();
        }

        public IActionResult CreateAdvertisementSuccess(Advertisement advertisement)
        {
            var existingadvertisement = _storeContext.Advertisements.SingleOrDefault(p=>p.ProductId ==advertisement.ProductId);
            if(existingadvertisement != null)
            {
                TempData["Message"] = "Product already had ad";
                return RedirectToAction("CreateAdvertisement");
            }
            if (advertisement.BannerImage == null)
            {
                TempData["Message"] = "Banner image is not null!";
                return RedirectToAction("CreateAdvertisement");
            }
            _storeContext.Add(advertisement);
            _storeContext.SaveChanges();
            return RedirectToAction("AdvertisementManagement");
        }

        public IActionResult EditAdvertisement(int advertisementId)
        {
            var advertisement = _storeContext.Advertisements.Find(advertisementId);
            var product = _storeContext.Products.Select(p => new { p.ProductId, p.Name });
            ViewBag.product = new SelectList(product, "ProductId", "Name");
            return View(advertisement);
        }

        public IActionResult EditAdvertisementSuccess(Advertisement advertisement)
        {
            var existingadvertisement = _storeContext.Advertisements.SingleOrDefault(p => p.Id == advertisement.Id);
            if (existingadvertisement != null)
            {
                existingadvertisement.ProductId = advertisement.ProductId;
                existingadvertisement.BannerImage = advertisement.BannerImage;
                existingadvertisement.Title = advertisement.Title;
                existingadvertisement.Description = advertisement.Description;

                _storeContext.Update(existingadvertisement);
                _storeContext.SaveChanges();
            }
            
            return RedirectToAction("AdvertisementManagement");
        }

        public IActionResult DeleteAdvertisement(int advertisementId)
        {
            var advertisement = _storeContext.Advertisements.Include(p=>p.Product).SingleOrDefault(p=>p.Id==advertisementId);
            return View(advertisement);
        }

        public IActionResult DeleteAdvertisementSuccess(int Id)
        {
            var existingadvertisement = _storeContext.Advertisements.Find(Id);
            _storeContext.Remove(existingadvertisement);
            _storeContext.SaveChanges();
            return RedirectToAction("AdvertisementManagement");
        }

        public IActionResult WaitOrder()
        {
            var waitorder = _storeContext.Orders.Where(p => p.Status.Equals("Chờ xác nhận")).Include(p => p.OrderDetails).ThenInclude(p=>p.Product).Include(p=>p.Customer).ThenInclude(p=>p.Account).OrderByDescending(p=>p.OrderDate).ToList();
            return View(waitorder);
        }

        public IActionResult ConfirmOrder(int id)
        {
            var order = _storeContext.Orders.Find(id);
            order.Status = "Đã xác nhận";
            _storeContext.Update(order);
            _storeContext.SaveChanges();

            var orderdetail = _storeContext.OrderDetails.Where(p => p.OrderId == order.OrderId).ToList();
            foreach(var item in orderdetail)
            {
                var product = _storeContext.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                product.StockQuantity -= item.Quantity;
                _storeContext.Products.Update(product);
            }
            _storeContext.SaveChanges();
            return RedirectToAction("OrderManagement");
        }
        public IActionResult DeleteOrder([FromForm, Required(ErrorMessage = "Cần nhập lý do hủy")] string Description, [FromForm] int OrderId)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Cần nhập lý do hủy";
                return RedirectToAction("WaitOrder");
            }
            var order = _storeContext.Orders.Find(OrderId);
            order.Status = "Đơn hàng bị hủy";
            order.Description = Description;
            _storeContext.Update(order);
            _storeContext.SaveChanges();
            return RedirectToAction("OrderManagement");
        }

        public IActionResult OrderManagement()
        {
            var order = _storeContext.Customers.Where(p=>p.Account.Role.Equals("KH")).Include(p=>p.Account).Include(p => p.Orders).ThenInclude(p => p.OrderDetails).ThenInclude(p => p.Product).ToList();
            return View(order);
        }
        public IActionResult DiscountManagement()
        {
            var discount = _storeContext.Discounts.ToList();
            return View(discount);
        }
        public IActionResult CreateDiscount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateDiscount(Discount discount)
        {
            var Discount = _storeContext.Discounts.SingleOrDefault(p => p.DiscountName.Equals(discount.DiscountName));
            if(Discount != null)
            {
                TempData["Message"] = "Discount has already had name";
                return View();
            }
            _storeContext.Add(discount);
            _storeContext.SaveChanges();
            TempData["Message"] = "Add discount success";
            return RedirectToAction("DiscountManagement");
        }

        public IActionResult Revenue()
        {
            return View();
        }
        public IActionResult FindProduct(string name)
        {
            var product = _storeContext.Products.Include(p => p.Suppliers).Where(p => p.Name.Contains(name)).ToList();
            return PartialView("FindProduct",product);
        }
    }
}
