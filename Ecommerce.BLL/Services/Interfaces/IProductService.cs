using Common.Result;
using Common.Settings;
using Ecommerce.BLL.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<PagedResult<ProductDTO>>> GetAllAsync(ProductQueryParams query);
        Task<Result<ProductDTO>> GetByIdAsync(int id);
        Task<Result<ProductDTO>> CreateAsync(CreateProductDTO dto);
        Task<Result<ProductDTO>> UpdateAsync(int id, UpdateProductDTO dto);
        Task<Result> DeleteAsync(int id);
    }
}
