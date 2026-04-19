using Ecommerce.BLL.DTOs.Cart;
using Ecommerce.DAL;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Validators.Cart
{
    public class UpdateCartDtoValidator : AbstractValidator<UpdateCartDTO>
    {
        public UpdateCartDtoValidator()
        {
            RuleFor(x => x.Product).GreaterThan(0).WithMessage("Invalid product.");
           
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be >= 0.");
        }
    }
}
