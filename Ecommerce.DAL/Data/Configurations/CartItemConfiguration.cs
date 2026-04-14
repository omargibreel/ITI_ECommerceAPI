using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL.Data.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(x=>x.Cart)
                .WithMany(x=>x.Items)
                .HasForeignKey(x=>x.CartId)
                .OnDelete(DeleteBehavior.Cascade);


            // this is relation 1:M between product and cartItem , cartItem Can have only one product but product can be in many cartItems
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            


            builder.HasIndex(ci => new { ci.CartId, ci.ProductId })
              .IsUnique()
              .HasDatabaseName("IX_CartItems_CartId_ProductId");

            builder.Property(ci => ci.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}
