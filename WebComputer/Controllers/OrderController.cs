using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
            
            var existcartitem = _storecontext.CartItems.SingleOrDefault(p=>p.CartId == cart.CartId && p.ProductId==productId);

            if (existcartitem != null)
            {
                existcartitem.Quantity = existcartitem.Quantity+1;
                _storecontext.CartItems.Update(existcartitem);
                _storecontext.SaveChanges();
                return RedirectToAction("Index", "HomePage");
            }

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
            ViewBag.giamgia = 0;
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
            ViewBag.cartId = cart.CartId;
            var cartItem = _storecontext.CartItems.Where(p=>p.CartId == cart.CartId).Include(p=>p.Product).ToList();
            var discount = _storecontext.Discounts.SingleOrDefault(c => c.DiscountId == cart.DiscountId);
            ViewBag.cartItem = cartItem;
     
            ViewBag.tonggia = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Sum(p => p.Product.Price * p.Quantity);
            if (discount != null)
            {
                if (ViewBag.tonggia < discount.Condition)
                {
                    cart.DiscountId = null;
                    _storecontext.Update(cart);
                    _storecontext.SaveChanges();
                    return RedirectToAction("Cart");
                }
                ViewBag.giamgia = ViewBag.tonggia * (discount.DiscountPercent / 100);
                ViewBag.thongtin = discount.Description;
            }
            ViewBag.thanhtien =ViewBag.tonggia-ViewBag.giamgia;
            return View();
        }

        public IActionResult AddDiscount(AddDiscountModel addDiscount)
        {
            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            var cartItem = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Include(p => p.Product).ToList();
            decimal tonggia = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Sum(p => p.Product.Price *p.Quantity);
            var discount = _storecontext.Discounts.SingleOrDefault(p => p.DiscountName.Equals(addDiscount.name));
            if (discount == null)
            {
                TempData["Message"] = "Invalid code";
                return RedirectToAction("Cart");
            }
            if (discount==null ||tonggia < discount.Condition)
            {
                TempData["Message"] = "The order does not qualify for the discount";
                return RedirectToAction("Cart");
            }
            cart.DiscountId = discount.DiscountId;
            _storecontext.Update(cart);
            _storecontext.SaveChanges();
            return RedirectToAction("Cart");
        }

        public IActionResult DeleteCart(int cartid, int cartitemid)
        {
            var cartitem = _storecontext.CartItems.Include(p=>p.Cart).SingleOrDefault(p => p.CartItemId == cartitemid && p.Cart.CartId ==cartid);
            _storecontext.Remove(cartitem);
            _storecontext.SaveChanges();
            decimal tonggia = _storecontext.CartItems.Where(p => p.CartId == cartid).Sum(p => p.Product.Price * p.Quantity);
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CartId == cartid);
            var discount = _storecontext.Discounts.Find(cart.DiscountId);
            if(discount != null)
            {
                if(discount.Condition > tonggia)
                {
                    cart.DiscountId = null;
                }
            }
            _storecontext.Carts.Update(cart);
            _storecontext.SaveChanges();
            return RedirectToAction("Cart","Order");
        }
        public IActionResult OrderCheck()
        {
            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            var cartItem = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Include(p => p.Product).ToList();
            var discount = _storecontext.Discounts.SingleOrDefault(p => p.DiscountId == cart.DiscountId);
            decimal discountamount = 0;
            ViewBag.tonggia = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Sum(p => p.Product.Price * p.Quantity);
            ViewBag.giamgia = 0;
            ViewBag.thanhtien = 0;
            if (discount != null)
            {
                ViewBag.giamgia = ViewBag.tonggia * (discount.DiscountPercent / 100);
                ViewBag.thongtin = discount.Description;
            }
            ViewBag.thanhtien = ViewBag.tonggia - ViewBag.giamgia;
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
                Status = "Chờ xác nhận",
                OrderDate = DateTime.UtcNow,
                CustomerId = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name)).CustomerId
            };
            _storecontext.Orders.Add(order);
            _storecontext.SaveChanges();

            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
            var cartItem = _storecontext.CartItems.Where(p => p.CartId == cart.CartId).Include(p => p.Product).ToList();
            
            decimal? totalamount = 0;
            decimal? discountamount = 0;
            decimal? thanhtien = 0;
            var discount = _storecontext.Discounts.SingleOrDefault(p => p.DiscountId == cart.DiscountId);

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

            }
            if(discount != null)
            {
                discountamount = totalamount * (discount.DiscountPercent / 100);
                order.DiscountId = discount.DiscountId;
            }
            thanhtien = totalamount-discountamount;
            order.Total = totalamount;
            order.DiscountValue = discountamount;
            order.TotalAmount = thanhtien;
            _storecontext.Orders.Update(order);
            _storecontext.SaveChanges();

            cart.DiscountId = null;
            _storecontext.Update(cart);
            _storecontext.CartItems.RemoveRange(cartItem);
            _storecontext.SaveChanges();
            TempData["Message"] = "Order success";
            return RedirectToAction("Index", "HomePage");
        }

        public IActionResult OrderDetail()
        {
            ViewBag.cartitemcount = 0;
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "You need to sign in to check order history";
                return RedirectToAction("Index", "HomePage");
            }
            var accountid = _storecontext.Accounts.SingleOrDefault(p => p.Email == User.Identity.Name).AccountId;
            ViewBag.cartitemcount = _storecontext.Customers.Where(x => x.AccountId == accountid).Include(p => p.Carts).ThenInclude(a => a.CartItems).SelectMany(c => c.Carts).SelectMany(d => d.CartItems).Count();
            var customer = _storecontext.Customers.FirstOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var order = _storecontext.Orders.Where(p => p.CustomerId == customer.CustomerId).Include(p=>p.OrderDetails).ThenInclude(p=>p.Product).Include(p=>p.Discount).OrderByDescending(p=>p.OrderDate);
            
            return View(order);
        }

        public IActionResult DeleteOrder([FromForm, Required(ErrorMessage = "Cần nhập lý do hủy")] string Description, [FromForm] int OrderId)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Cần nhập lý do hủy";
                return RedirectToAction("OrderDetail");
            }
            var order = _storecontext.Orders.Find(OrderId);
            order.Status = "Đơn hàng bị hủy";
            order.Description = Description;
            _storecontext.Update(order);
            _storecontext.SaveChanges();
            return RedirectToAction("OrderDetail");            
        }
    }
}
