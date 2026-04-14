using Ecommerce.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class Cart: BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public ICollection<CartItem> Items { get; set; } = new HashSet<CartItem>();
    }
}
