using Ecommerce.DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //public AppDbContext() : base() { }

        public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }   
        public DbSet<CartItem> CartItems { get; set; }   
        public DbSet<Order> Orders { get; set; }   
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
