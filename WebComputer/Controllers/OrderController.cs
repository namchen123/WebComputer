using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebComputer.Models;

namespace WebComputer.Controllers
{
    public class OrderController : Controller
    {
        private readonly ComputerStoreContext _storecontext;
        public OrderController(ComputerStoreContext storecontext)
        {
            _storecontext = storecontext;
        }

        public IActionResult AddToCart(int productId)
        {
            if(!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "You need to sign in to add to cart";
                return RedirectToAction("ProductDetail", "Product", new {productId = productId});
            }
            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
                   
            CartItem cartItem = new CartItem();
            cartItem.CartId = _storecontext.Carts.SingleOrDefault(p=>p.CustomerId == cart.CustomerId).CartId;
            cartItem.ProductId = productId;
            cartItem.Quantity = 1;
            _storecontext.CartItems.Add(cartItem);
            _storecontext.SaveChanges();
            return RedirectToAction("Index","HomePage");
        }
        public IActionResult Cart()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "You need to sign in to check the cart";
                return RedirectToAction("Index", "HomePage");
            }
            ViewBag.tonggia = 0;
            ViewBag.thanhtien = 0;
            var accountid = _storecontext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
            ViewBag.cartitemcount = 0;
            int cartitemcount = _storecontext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            ViewBag.cartitemcount = cartitemcount;
            if (cartitemcount == 0)
            {
                return View();
            }
            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            var cartItem = _storecontext.CartItems.Where(p=>p.CartId == cart.CartId).Include(p=>p.Product).ToList();

            ViewBag.tonggia = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Sum(p => p.Product.Price);

            return View(cartItem);
        }
        public IActionResult DeleteCart(int cartid, int cartitemid)
        {
            var cartitem = _storecontext.CartItems.SingleOrDefault(p => p.CartItemId == cartitemid && p.Cart.CartId ==cartid);
            _storecontext.Remove(cartitem);
            _storecontext.SaveChanges();
            return RedirectToAction("Cart","Order");
        }
        public IActionResult OrderCheck()
        {
            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            var cartItem = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Include(p => p.Product).ToList();

            ViewBag.tonggia = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Sum(p => p.Product.Price);

            ViewBag.Name = customer.FirstName + " " +customer.LastName;
            ViewBag.Adress = customer.Address;
            ViewBag.Phone = customer.Phone;
            return View();
        }
        [HttpPost]
        public IActionResult CreateOrder(OrderViewModel orderView)
        {
            Order order = new Order
            {
                Adress = orderView.Adress,
                Phone = orderView.Phone,
                Email = orderView.Email,
                Status = "Done",
                OrderDate = DateTime.UtcNow,
                CustomerId = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name)).CustomerId
            };
            _storecontext.Orders.Add(order);
            _storecontext.SaveChanges();

            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            var cartItem = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Include(p => p.Product).ToList();
            
            decimal totalamount = 0;

            foreach (var item in cartItem)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };
                totalamount += detail.UnitPrice * detail.Quantity;
                _storecontext.OrderDetails.Add(detail);

                var product = _storecontext.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                product.StockQuantity -= item.Quantity;
                _storecontext.Products.Update(product);
            }
            order.TotalAmount = totalamount;
            _storecontext.Orders.Update(order);
            _storecontext.SaveChanges();

            _storecontext.CartItems.RemoveRange(cartItem);
            _storecontext.SaveChanges();
            TempData["Message"] = "Order success";
            return RedirectToAction("Index", "HomePage");
        }

        public IActionResult OrderDetail()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "You need to sign in to check order history";
                return RedirectToAction("Index", "HomePage");
            }
            var customer = _storecontext.Customers.FirstOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var order = _storecontext.Orders.Where(p => p.CustomerId == customer.CustomerId).Include(p=>p.OrderDetails).ThenInclude(p=>p.Product);
            return View(order);
        }
    }
}
