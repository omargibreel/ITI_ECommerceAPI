using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Cart
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
