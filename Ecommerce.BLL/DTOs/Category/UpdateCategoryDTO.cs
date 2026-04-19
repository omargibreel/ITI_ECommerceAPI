using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Category
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? ImageFile { get; set; }
        public bool RemoveExistingImage { get; set; } = false;
    }
}
