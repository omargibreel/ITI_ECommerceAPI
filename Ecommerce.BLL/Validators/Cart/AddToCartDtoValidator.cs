using Ecommerce.BLL.DTOs.Cart;
using Ecommerce.DAL;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Validators.Cart
{
    public class AddToCartDtoValidator : AbstractValidator<AddToCartDTO>
    {
        public AddToCartDtoValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Invalid product.");
           
            RuleFor(x => x.Quantity).InclusiveBetween(1, 100).WithMessage("Quantity must be 1 - 100.");
        }
    }
}
