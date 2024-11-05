using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class Advertisement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BannerImage { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
