using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class ProductSpecification
    {
        public int ProductSpecificationId { get; set; }
        public int? ProductId { get; set; }
        public int? SpecificationId { get; set; }
        public string SpecificationValue { get; set; } = null!;

        public virtual Product? Product { get; set; }
        public virtual Specification? Specification { get; set; }
    }
}
