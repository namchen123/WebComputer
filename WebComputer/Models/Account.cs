using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class Account
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
        }

        public int AccountId { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
