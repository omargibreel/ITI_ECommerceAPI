using AutoMapper;
using Common.GeneralResuls;
using Common.Result;
using Common.Settings;
using Ecommerce.BLL.DTOs.Product;
using Ecommerce.BLL.Services.Interfaces;
using Ecommerce.DAL;
using Ecommerce.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ecommerce.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;


        public ProductService(IUnitOfWork uow,
                                IMapper mapper,
                                IPhotoService
                                photoService )
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }


        public async Task<Result<PagedResult<ProductDTO>>> GetAllAsync(ProductQueryParams query)
        {
            var repo = _uow.GetRepository<Product>();
            Expression<Func<Product, bool>>? condition = null;

            if (query.CategoryId.HasValue && !string.IsNullOrEmpty(query.Name))
                condition = p => p.CategoryId == query.CategoryId && p.Name.Contains(query.Name);

            else if (query.CategoryId.HasValue)
                condition = p => p.CategoryId == query.CategoryId;

            else if (!string.IsNullOrEmpty(query.Name))
                condition = p => p.Name.Contains(query.Name);

            var (items, total) = await repo.GetPagedAsync( condition, query.PageNumber, query.PageSize, p => p.Category);
            var dtos = _mapper.Map<IEnumerable<ProductDTO>>(items);
            return Result<PagedResult<ProductDTO>>.Success(new
           PagedResult<ProductDTO>
            {
                Items = dtos.ToList(),
                TotalCount = total,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }
        public async Task<Result<ProductDTO>> GetByIdAsync(int id)
        {
            var product = await _uow.GetRepository<Product>()
            .GetByIdNoTracking(id, p => p.Category);
            if (product is null) return Result<ProductDTO>.NotFound("Product", id);
            return Result<ProductDTO>.Success(_mapper.Map<ProductDTO>(product));
        }
        public async Task<Result<ProductDTO>> CreateAsync(CreateProductDTO dto)
        {
            var category = await _uow.GetRepository<Category>().GetById(dto.CategoryId);
            if (category is null)
                return Result<ProductDTO>.Fail(Error.Validation("CategoryId", "Invalid category id."));

            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.UtcNow;
            if (dto.ImageFile != null)
            {
                var uploadResult = await _photoService.UploadAsync(dto.ImageFile,
               "products");
                if (uploadResult.IsFailure) return
               Result<ProductDTO>.Fail(uploadResult.Errors!);
                product.ImageURL = uploadResult.Value;
            }
            _uow.GetRepository<Product>().Add(product);
            await _uow.SaveChangesAsync();
            return await GetByIdAsync(product.Id);
        }
        public async Task<Result<ProductDTO>> UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await _uow.GetRepository<Product>().GetById(id);
            if (product is null)
                return Result<ProductDTO>.NotFound("Product", id);

            var category = await _uow.GetRepository<Category>().GetById(dto.CategoryId);
            if (category is null)
                return Result<ProductDTO>.Fail(Error.Validation("CategoryId", "Invalid category id."));

            if (dto.RemoveExistingImage && product.ImageURL != null)
            {
                await _photoService.DeleteAsync(product.ImageURL);
                product.ImageURL = null;
            }
            if (dto.ImageFile != null)
            {
                if (product.ImageURL != null) await
               _photoService.DeleteAsync(product.ImageURL);
                var upload = await _photoService.UploadAsync(dto.ImageFile,
               "products");
                if (upload.IsFailure) return
               Result<ProductDTO>.Fail(upload.Errors!);
                product.ImageURL = upload.Value;
            }

            _mapper.Map(dto, product);
            product.UpdatedAt = DateTime.UtcNow;
            _uow.GetRepository<Product>().Update(product);

            await _uow.SaveChangesAsync();
            return await GetByIdAsync(id);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var product = await _uow.GetRepository<Product>().GetById(id);

            if (product is null) 
                return Result.NotFound("Product", id);

            var isReferencedByOrders = (await _uow.GetRepository<OrderItem>()
                .GetAll(x => x.ProductId == id)).Any();
            if (isReferencedByOrders)
                return Result.Fail(Error.Conflict("Cannot delete product because it is referenced by order items."));

            if (product.ImageURL != null) 
                await _photoService.DeleteAsync(product.ImageURL);

            _uow.GetRepository<Product>().Delete(product);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
    }
}

