using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageURL { get; set; }


        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
