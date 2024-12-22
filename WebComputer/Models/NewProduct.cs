using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace WebComputer.Models
{
    public class NewProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Không âm.")]
        public int StockQuantity { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
    }
}
