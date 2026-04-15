using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Cart
{
    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
