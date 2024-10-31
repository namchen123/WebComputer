using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebComputer.Models;

namespace WebComputer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly ComputerStoreContext _storeContext;

        public ProductApiController(ComputerStoreContext storeContext)
        {
            this._storeContext = storeContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var product = _storeContext.Products.Include(p=>p.CartItems).ToList();
            return Ok(product);
        }
        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] int id)
        {
            var product = _storeContext.Products.Include(p => p.CartItems).Where(p=>p.CategoryId==id).ToList();
            return Ok(product);
        }
    }
}
