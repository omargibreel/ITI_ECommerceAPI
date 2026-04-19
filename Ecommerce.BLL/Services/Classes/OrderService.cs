using AutoMapper;
using Common.GeneralResuls;
using Common.Result;
using Ecommerce.BLL.DTOs.Order;
using Ecommerce.BLL.Services.Interfaces;
using Ecommerce.DAL;
using Ecommerce.DAL.Data.Models;
using Ecommerce.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Result<OrderDTO>> PlaceOrderAsync(string userId,PlaceOrderDTO dto)
        {
            var carts = await _uow.GetRepository<Cart>().GetAll(
            c => c.UserId == userId, c => c.Items);
            var cart = carts.FirstOrDefault();
            if (cart is null || !cart.Items.Any())
                return Result<OrderDTO>.Fail(Error.Validation("Cart is empty."));

            await LoadProductsForCartItemsAsync(cart.Items);

            // Validate stock for all items
            foreach (var item in cart.Items)
            {
                if (item.Product.Stock < item.Quantity)
                    return Result<OrderDTO>.Fail(Error.Validation(
                    $"Insufficient stock for '{item.Product.Name}'."));
            }
            var order = new Order
            {
                UserId = userId,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity),
                OrderItems = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price,
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            };
            // Deduct stock
            foreach (var item in cart.Items)
            {
                item.Product.Stock -= item.Quantity;
                _uow.GetRepository<Product>().Update(item.Product);
            }
            _uow.GetRepository<Order>().Add(order);
            // Clear the cart
            foreach (var item in cart.Items.ToList())
                _uow.GetRepository<CartItem>().Delete(item);
            await _uow.SaveChangesAsync();
            return await GetOrderByIdAsync(userId, order.Id);
        }
        public async Task<Result<IEnumerable<OrderDTO>>> GetOrderHistoryAsync(string userId)
        {
            var orders = await _uow.GetRepository<Order>().GetAll(
            o => o.UserId == userId, o => o.OrderItems);

            await LoadProductsForOrderItemsAsync(orders.SelectMany(o => o.OrderItems));

            return Result<IEnumerable<OrderDTO>>.Success(_mapper.Map<IEnumerable<OrderDTO>>(orders)
           );
        }
        public async Task<Result<OrderDTO>> GetOrderByIdAsync(string userId, int orderId)
        {
            var orders = await _uow.GetRepository<Order>().GetAll(
            o => o.UserId == userId && o.Id == orderId,
            o => o.OrderItems
            );

            var order = orders.FirstOrDefault();
            if (order is null) return Result<OrderDTO>.NotFound("Order", orderId);

            await LoadProductsForOrderItemsAsync(order.OrderItems);

            return Result<OrderDTO>.Success(_mapper.Map<OrderDTO>(order));
        }

        private async Task LoadProductsForCartItemsAsync(IEnumerable<CartItem> items)
        {
            var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            if (!productIds.Any())
                return;

            var products = await _uow.GetRepository<Product>()
                .GetAll(p => productIds.Contains(p.Id));
            var productMap = products.ToDictionary(p => p.Id);

            foreach (var item in items)
            {
                if (productMap.TryGetValue(item.ProductId, out var product))
                    item.Product = product;
            }
        }

        private async Task LoadProductsForOrderItemsAsync(IEnumerable<OrderItem> items)
        {
            var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            if (!productIds.Any())
                return;

            var products = await _uow.GetRepository<Product>()
                .GetAll(p => productIds.Contains(p.Id));
            var productMap = products.ToDictionary(p => p.Id);

            foreach (var item in items)
            {
                if (productMap.TryGetValue(item.ProductId, out var product))
                    item.Product = product;
            }
        }
    }
}
