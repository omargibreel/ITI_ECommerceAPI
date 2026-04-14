using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL.Data.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4,  
    }
}
