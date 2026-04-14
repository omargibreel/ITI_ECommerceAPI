using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Ecommerce.DAL.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
