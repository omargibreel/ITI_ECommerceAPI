using Common.Result;
using Ecommerce.BLL.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Interfaces
{
    public interface ICartService
    {
        Task<Result<CartDTO>> GetCartAsync(string userId);
        Task<Result<CartDTO>> AddToCartAsync(string userId, AddToCartDTO dto);
        Task<Result<CartDTO>> UpdateQuantityAsync(string userId, UpdateCartDTO dto);
        Task<Result> RemoveItemAsync(string userId, int productId);
        Task<Result> ClearCartAsync(string userId);
    }
}
