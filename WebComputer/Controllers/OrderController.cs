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
        public IActionResult Cart(int itemcount)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "You need to sign in to check the cart";
                return RedirectToAction("Index", "HomePage");
            }
            if (itemcount == 0)
            {
                return View();
            }
            var customer = _storecontext.Customers.SingleOrDefault(p => p.Account.Email.Equals(User.Identity.Name));
            var cart = _storecontext.Carts.SingleOrDefault(c => c.CustomerId == customer.CustomerId);

            var cartItem = _storecontext.CartItems.Where(p=>p.CartId == cart.CartId).Include(p=>p.Product).ToList();

            return View(cartItem);
        }
    }
}
