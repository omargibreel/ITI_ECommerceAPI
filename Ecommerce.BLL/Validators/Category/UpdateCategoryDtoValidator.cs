using Ecommerce.BLL.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Validators.Category
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile!)
                .Must(f => f.Length <= 5 * 1024 * 1024).WithMessage("Image size cannot exceed 5MB.")
                .Must(f => new[] { ".jpg", ".jpeg", ".png", ".webp" }

               .Contains(Path.GetExtension(f.FileName).ToLowerInvariant()))
                .WithMessage("Invalid image format.");
            });


        }
    }
}