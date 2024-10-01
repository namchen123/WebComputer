using System;
using System.Collections.Generic;

namespace WebComputer.Models
{
    public partial class Specification
    {
        public Specification()
        {
            ProductSpecifications = new HashSet<ProductSpecification>();
        }

        public int SpecificationId { get; set; }
        public string SpecificationName { get; set; } = null!;

        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }
}
