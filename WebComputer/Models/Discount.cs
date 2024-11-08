using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        public int DiscountId { get; set; }
        public string DiscountName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountMoney { get; set; }
        public decimal? Condition { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
