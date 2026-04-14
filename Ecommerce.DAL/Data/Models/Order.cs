using Ecommerce.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public decimal TotalAmount  { get; set; }
        public OrderStatus  Status { get; set; } = OrderStatus.Pending;
        public string Address { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();  
    }
}
