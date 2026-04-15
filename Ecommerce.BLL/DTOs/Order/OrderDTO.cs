using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
