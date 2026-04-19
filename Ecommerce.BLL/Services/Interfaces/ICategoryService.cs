using Common.Result;
using Ecommerce.BLL.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDTO>>> GetAllAsync();
        Task<Result<CategoryDTO>> GetByIdAsync(int id);
        Task<Result<CategoryDTO>> CreateAsync(CreateCategoryDTO dto);
        Task<Result<CategoryDTO>> UpdateAsync(int id, UpdateCategoryDTO dto);
        Task<Result> DeleteAsync(int id);
    }
}
