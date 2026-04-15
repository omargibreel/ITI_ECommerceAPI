using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Order
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
