using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.DTOs.Auth
{
    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
