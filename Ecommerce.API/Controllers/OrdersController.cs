using Ecommerce.API.Extensions;
using Ecommerce.BLL.DTOs.Order;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) => _orderService =
       orderService;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDTO dto)
        => (await _orderService.PlaceOrderAsync(UserId, dto)).ToActionResult();

        [HttpGet]
        public async Task<IActionResult> GetOrderHistory()
        => (await _orderService.GetOrderHistoryAsync(UserId)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        => (await _orderService.GetOrderByIdAsync(UserId, id)).ToActionResult();
    }
}
