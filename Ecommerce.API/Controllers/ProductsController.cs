using Ecommerce.API.Extensions;
using Ecommerce.BLL.DTOs.Product;
using Ecommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
            => _productService = productService;


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryParams  query)
        => (await _productService.GetAllAsync(query)).ToActionResult();


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        => (await _productService.GetByIdAsync(id)).ToActionResult();


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO dto)
        => (await _productService.CreateAsync(dto)).ToActionResult();


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDTO dto)
        => (await _productService.UpdateAsync(id, dto)).ToActionResult();


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        => (await _productService.DeleteAsync(id)).ToActionResult();
    }
}
