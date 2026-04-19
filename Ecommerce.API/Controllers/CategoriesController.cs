using Ecommerce.API.Extensions;
using Ecommerce.BLL.DTOs.Category;
using Ecommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
            => _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        => (await _categoryService.GetAllAsync()).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        => (await _categoryService.GetByIdAsync(id)).ToActionResult();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDTO dto)
        => (await _categoryService.CreateAsync(dto)).ToActionResult();

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCategoryDTO dto)
        => (await _categoryService.UpdateAsync(id, dto)).ToActionResult();

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        => (await _categoryService.DeleteAsync(id)).ToActionResult();
    }
}
