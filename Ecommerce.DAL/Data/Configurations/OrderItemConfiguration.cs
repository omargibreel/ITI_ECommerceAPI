using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(x=>x.Order)
                .WithMany(x=>x.OrderItems)
                .HasForeignKey(x=>x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x=>x.Product)
                .WithMany()
                .HasForeignKey(x=>x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
