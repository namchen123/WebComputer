using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? DiscountId { get; set; }
        public decimal? Total { get; set; }
        public decimal? DiscountValue { get; set; }
        public string? Description { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Discount? Discount { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
