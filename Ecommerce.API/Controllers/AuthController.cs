using Ecommerce.API.Extensions;
using Ecommerce.BLL.DTOs.Auth;
using Ecommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
            => _authService = authService;


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        => (await _authService.RegisterAsync(dto)).ToActionResult();


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        => (await _authService.LoginAsync(dto)).ToActionResult();
    }
}
