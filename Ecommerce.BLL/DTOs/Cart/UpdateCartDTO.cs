using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Cart
{
    public class UpdateCartDTO
    {
        public int Product { get; set; }
        public int Quantity { get; set; }
    }
}
