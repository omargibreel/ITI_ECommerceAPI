using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Auth
{
    public class AuthDTO
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime Expiry { get; set; }
    }
}
