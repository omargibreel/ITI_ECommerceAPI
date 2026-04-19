using AutoMapper;
using Common.Result;
using Ecommerce.BLL.DTOs.Order;
using Ecommerce.BLL.Services.Classes;
using Ecommerce.DAL;
using Ecommerce.DAL.Data.Models;
using Ecommerce.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderDTO>> PlaceOrderAsync(string userId, PlaceOrderDTO dto);
        Task<Result<IEnumerable<OrderDTO>>> GetOrderHistoryAsync(string userId);
        Task<Result<OrderDTO>> GetOrderByIdAsync(string userId, int orderId);
    }
}
