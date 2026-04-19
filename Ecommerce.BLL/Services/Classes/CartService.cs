using AutoMapper;
using Common.GeneralResuls;
using Common.Result;
using Ecommerce.BLL.DTOs.Cart;
using Ecommerce.BLL.Services.Interfaces;
using Ecommerce.DAL;
using Ecommerce.DAL.UnitOfWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.BLL.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        private async Task<Cart?> GetOrCreateCartAsync(string userId)
        {
            var carts = await _uow.GetRepository<Cart>()
            .GetAll(c => c.UserId == userId, c => c.Items);

            var cart = carts.FirstOrDefault();

            if (cart != null && cart.Items.Any())
            {
                var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToList();
                var products = await _uow.GetRepository<Product>()
                    .GetAll(p => productIds.Contains(p.Id));

                var productMap = products.ToDictionary(p => p.Id);
                foreach (var item in cart.Items)
                {
                    if (productMap.TryGetValue(item.ProductId, out var product))
                        item.Product = product;
                }
            }

            if (cart is null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
                _uow.GetRepository<Cart>().Add(cart);
                await _uow.SaveChangesAsync();
            }
            return cart;
        }
        public async Task<Result<CartDTO>> GetCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return Result<CartDTO>.Success(_mapper.Map<CartDTO>(cart));
        }
        public async Task<Result<CartDTO>> AddToCartAsync(string userId, AddToCartDTO dto)
        {
            var product = await _uow.GetRepository<Product>().GetById(dto.ProductId);
            if (product is null)
                return Result<CartDTO>.NotFound("Product", dto.ProductId);

            if (product.Stock < dto.Quantity)
                return Result<CartDTO>.Fail(Error.Validation("Insufficient stock."));



            var cart = await GetOrCreateCartAsync(userId);
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);

            if (existing != null)
                existing.Quantity += dto.Quantity;

            else
                cart.Items.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CartId = cart.Id,
                    CreatedAt = DateTime.UtcNow
                });

            _uow.GetRepository<Cart>().Update(cart);
            await _uow.SaveChangesAsync();
            return await GetCartAsync(userId);
        }
        public async Task<Result<CartDTO>> UpdateQuantityAsync(string userId, UpdateCartDTO dto)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == dto.Product);

            if (item is null) 
                return Result<CartDTO>.NotFound("CartItem", dto.Product);

            if (dto.Quantity <= 0)
                _uow.GetRepository<CartItem>().Delete(item);

            else
            {
                item.Quantity = dto.Quantity;
                item.UpdatedAt = DateTime.UtcNow;
                _uow.GetRepository<CartItem>().Update(item);
            }
            await _uow.SaveChangesAsync();
            return await GetCartAsync(userId);
        }
        public async Task<Result> RemoveItemAsync(string userId, int productId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item is null) 
                return Result.NotFound("CartItem", productId);
            _uow.GetRepository<CartItem>().Delete(item);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result> ClearCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            foreach (var item in cart.Items.ToList())
                _uow.GetRepository<CartItem>().Delete(item);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
    }
}
