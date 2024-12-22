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
        [HttpPost("UpdateCartItemQuantity")]
        public IActionResult UpdateCartItemQuantity([FromBody] CartItemUpdateModel model)
        {
            try
            {
                // Lấy thông tin sản phẩm trong giỏ hàng
                var cartitem = _storeContext.CartItems
                    .Include(c => c.Product) // Đảm bảo Product được bao gồm trong truy vấn
                    .SingleOrDefault(p => p.CartId == model.cartid && p.ProductId == model.productId);

                if (cartitem == null)
                {
                    return NotFound(new { message = "Sản phẩm không có trong giỏ hàng" });
                }

                // Kiểm tra số lượng có hợp lệ không
                if (model.quantity <= 0)
                {
                    return BadRequest(new { message = "Số lượng phải lớn hơn 0" });
                }

                // Kiểm tra nếu Product trong cartitem là null
                if (cartitem.Product == null)
                {
                    return StatusCode(500, new { message = "Không tìm thấy sản phẩm liên kết với giỏ hàng." });
                }

                // Cập nhật số lượng sản phẩm
                cartitem.Quantity = model.quantity;
                _storeContext.SaveChanges();


                // Tính lại tổng giá trị
                decimal tonggia = _storeContext.CartItems.Where(p => p.CartId == model.cartid).Sum(p => p.Product.Price * p.Quantity);
                var totalPrice = cartitem.Product.Price * cartitem.Quantity;
                var cart = _storeContext.Carts.Include(p=>p.Discount).SingleOrDefault(p=>p.CartId==model.cartid);
                string description = null;
                Discount discount = null;
                decimal discountpercent = 0;
                if (cart.DiscountId != null)
                {
                    discount = _storeContext.Discounts.Find(cart.DiscountId);
                    if(discount.Condition < tonggia)
                    {
                        discountpercent = cart.Discount.DiscountPercent ?? 0;
                        description = discount.Description;
                    } else
                    {
                        cart.DiscountId = null;
                        _storeContext.Carts.Update(cart);
                        _storeContext.SaveChanges();
                    }
                } 

                var discountmoney = _storeContext.CartItems.Where(p => p.CartId == model.cartid).Sum(p => p.Product.Price * p.Quantity) * ((discountpercent) / 100);
                var thanhtien = _storeContext.CartItems.Where(p => p.CartId == model.cartid).Sum(p => p.Product.Price * p.Quantity) - discountmoney;

                // Trả về kết quả
                return Ok(new { description, discountmoney = discountmoney.ToString("N0"), thanhtien = thanhtien.ToString("N0"), model.productId, quantity = cartitem.Quantity, totalPrice = totalPrice.ToString("N0") });
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết
                Console.WriteLine($"Lỗi: {ex.Message}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình cập nhật", error = ex.Message });
            }
        }
        [HttpGet("RevenueByMonth")]
        public ActionResult RevenueByMonth()
        {
            var revenueData = _storeContext.Orders
                .Where(o => o.Status.Equals("Thành công")) // Lọc các đơn hàng đã xác nhận
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month }) // Nhóm theo năm và tháng
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalAmount) // Tổng doanh thu
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ToList();

            return Ok(revenueData); // Trả về dữ liệu dạng JSON
        }

        [HttpGet("RevenueByAllMonth")]
        public ActionResult RevenueByAllMonth()
        {
            // Lấy dữ liệu doanh thu của các tháng đã có đơn hàng
            var revenueData = _storeContext.Orders
                .Where(o => o.Status.Equals("Thành công") || o.Status.Equals("Đã xác nhận"))
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            // Tạo danh sách các tháng trong năm (1 đến 12)
            var currentYear = DateTime.Now.Year;
            var allMonths = Enumerable.Range(1, 12).Select(month => new
            {
                Year = currentYear,
                Month = month,
                Revenue = revenueData.FirstOrDefault(r => r.Month == month)?.Revenue ?? 0 // Gán 0 nếu không có dữ liệu
            }).ToList();

            return Ok(allMonths);
        }
        [HttpGet("RevenueByDateRange")]
        public IActionResult RevenueByDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return BadRequest("Vui lòng cung cấp đầy đủ ngày bắt đầu và ngày kết thúc.");
            }

            if (startDate > endDate)
            {
                return BadRequest("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
            }

            // Tạo danh sách các tháng trong khoảng thời gian
            var months = new List<(int Year, int Month)>();
            var current = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);

            while (current <= endDate.Value)
            {
                months.Add((current.Year, current.Month));
                current = current.AddMonths(1); // Chuyển sang tháng tiếp theo
            }

            // Lấy dữ liệu doanh thu từ cơ sở dữ liệu
            var revenueData = _storeContext.Orders
                .Where(o => (o.Status == "Thành công" || o.Status == "Đã xác nhận") &&
                            o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            // Kết hợp danh sách các tháng với dữ liệu doanh thu
            var result = months
                .Select(m => new
                {
                    Year = m.Year,
                    Month = m.Month,
                    Revenue = revenueData
                        .Where(r => r.Year == m.Year && r.Month == m.Month)
                        .Select(r => r.Revenue)
                        .DefaultIfEmpty(0) 
                        .First()
                })
                .ToList();

            return Ok(result);
        }
    }
}
