using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Cart
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ImageURL { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
