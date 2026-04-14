using Ecommerce.BLL.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Validators.Product
{
    public class UpdateProductDtoValidator :  AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Product name is required.")
               .MinimumLength(2)
               .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThanOrEqualTo(999999.99m);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile!)
                    .Must(f => f.Length <= 5 * 1024 * 1024)
                    .WithMessage("Image size cannot exceed 5MB.")
                    .Must(f =>
                    {
                        var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                        return new[] { ".jpg", ".jpeg", ".png", ".webp" }.Contains(ext);
                    })
                    .WithMessage("Only JPG, PNG, or WEBP images are allowed.");
            });
        }
    }
}
