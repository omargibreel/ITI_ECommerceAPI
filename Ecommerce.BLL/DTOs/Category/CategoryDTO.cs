using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageURL { get; set; }
        public int ProductCount { get; set; }  // useful for UI display
        public DateTime CreatedAt { get; set; }
    }
}
