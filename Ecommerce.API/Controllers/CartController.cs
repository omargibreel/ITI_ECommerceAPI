using Ecommerce.API.Extensions;
using Ecommerce.BLL.DTOs.Cart;
using Ecommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
            => _cartService = cartService;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetCart()
        => (await _cartService.GetCartAsync(UserId)).ToActionResult();

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
        => (await _cartService.AddToCartAsync(UserId, dto)).ToActionResult();

        [HttpPut("items")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartDTO dto)
        => (await _cartService.UpdateQuantityAsync(UserId, dto)).ToActionResult();

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        => (await _cartService.RemoveItemAsync(UserId, productId)).ToActionResult();

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        => (await _cartService.ClearCartAsync(UserId)).ToActionResult();
    }
}
