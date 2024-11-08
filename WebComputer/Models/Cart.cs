using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int CartId { get; set; }
        public int? CustomerId { get; set; }
        public int? DiscountId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Discount? Discount { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
