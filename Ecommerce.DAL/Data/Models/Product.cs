using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageURL { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
