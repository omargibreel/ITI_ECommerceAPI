using Common.Result;
using Ecommerce.BLL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthDTO>> RegisterAsync(RegisterDTO dto);
        Task<Result<AuthDTO>> LoginAsync(LoginDTO dto);
    }
}
