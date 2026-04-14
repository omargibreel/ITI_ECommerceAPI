using Ecommerce.BLL.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Validators.Product
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Price is unrealistically high.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid category must be selected.");

            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile!)
                    .Must(file => file.Length <= 5 * 1024 * 1024)
                    .WithMessage("Image size cannot exceed 5MB.")
                    .Must(file =>
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                        return allowedExtensions.Contains(ext);
                    })
                    .WithMessage("Only JPG, PNG, or WEBP images are allowed.");
            });
        }
    }
}
