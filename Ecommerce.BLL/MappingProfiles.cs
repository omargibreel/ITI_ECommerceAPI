using AutoMapper;
using Ecommerce.BLL.DTOs.Auth;
using Ecommerce.BLL.DTOs.Cart;
using Ecommerce.BLL.DTOs.Category;
using Ecommerce.BLL.DTOs.Order;
using Ecommerce.BLL.DTOs.Product;
using Ecommerce.DAL;
using Ecommerce.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class MappingProfiles : Profile
    {


        public MappingProfiles()
        {
            MapProduct();
            MapCategory();
            MapCart();
            MapOrder();
            MapAuth();
        }

        private void MapProduct()
        {
            CreateMap<Product, ProductDTO>()
           .ForMember(dest => dest.CategoryName,
               opt => opt.MapFrom(src => src.Category != null
                   ? src.Category.Name
                   : "Uncategorized"));

            // ── Create DTO → Entity ────────────────────────────
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }

        private void MapCategory()
        {
            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.ProductCount,
                opt => opt.MapFrom(src => src.Products.Count));

            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());


            CreateMap<UpdateCategoryDTO, Category>()
             .ForMember(dest => dest.ImageURL, opt => opt.Ignore())
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.Products, opt => opt.Ignore());
        }

        private void MapCart()
        {
            CreateMap<CartItem, CartItemDTO>()
           .ForMember(dest => dest.CartItemId,
               opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.ProductName,
               opt => opt.MapFrom(src => src.Product.Name))
           .ForMember(dest => dest.ImageURL,
               opt => opt.MapFrom(src => src.Product.ImageURL))
           .ForMember(dest => dest.UnitPrice,
               opt => opt.MapFrom(src => src.Product.Price))
           .ForMember(dest => dest.SubTotal,
               opt => opt.MapFrom(src => src.Product.Price * src.Quantity));


            CreateMap<Cart, CartDTO>()
         .ForMember(dest => dest.CartId,
             opt => opt.MapFrom(src => src.Id))

         .AfterMap((src, dest) =>
         {
             dest.Total = dest.Items.Sum(i => i.SubTotal);
         });
        }

        private void MapOrder()
        {
            CreateMap<OrderItem, OrderItemDTO>()
           .ForMember(dest => dest.ProductName,
               opt => opt.MapFrom(src => src.Product.Name))
           .ForMember(dest => dest.SubTotal,
               opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }

        private void MapAuth()
        {
            CreateMap<RegisterDTO, AppUser>()
          .ForMember(dest => dest.UserName,
              opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
          .ForMember(dest => dest.Cart, opt => opt.Ignore())
          .ForMember(dest => dest.Orders, opt => opt.Ignore());
        }
    }
}
