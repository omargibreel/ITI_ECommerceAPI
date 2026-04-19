using Ecommerce.BLL.DTOs.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Validators.Order
{
    public class PlaceOrderDtoValidator : AbstractValidator<PlaceOrderDTO>
    {
        public PlaceOrderDtoValidator()
        {
            RuleFor(x =>
           x.Address).NotEmpty().MaximumLength(500).WithMessage("Address required.");
        }
    }
}
