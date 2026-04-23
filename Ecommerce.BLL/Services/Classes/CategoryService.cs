using AutoMapper;
using Common.GeneralResuls;
using Common.Result;
using Ecommerce.BLL.DTOs.Category;
using Ecommerce.BLL.Services.Interfaces;
using Ecommerce.DAL;
using Ecommerce.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.BLL.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public CategoryService(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<Result<IEnumerable<CategoryDTO>>> GetAllAsync()
        {
            var cats = await _uow.GetRepository<Category>().GetAll(null, c => c.Products);
            return Result<IEnumerable<CategoryDTO>>.Success(_mapper.Map<IEnumerable<CategoryDTO>>(cats));
        }
        public async Task<Result<CategoryDTO>> GetByIdAsync(int id)
        {
            var cat = await _uow.GetRepository<Category>().GetByIdNoTracking(id, c => c.Products);
            if (cat is null)
                return Result<CategoryDTO>.NotFound("Category", id);

            return Result<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(cat));
        }
        public async Task<Result<CategoryDTO>> CreateAsync(CreateCategoryDTO dto)
        {
            var existingByName = await _uow.GetRepository<Category>()
                .GetAll(c => c.Name == dto.Name);
            if (existingByName.Any())
                return Result<CategoryDTO>.Fail(Error.Conflict("Category name already exists."));

            var cat = _mapper.Map<Category>(dto);
            cat.CreatedAt = DateTime.UtcNow;

            if (dto.ImageFile != null)
            {
                var upload = await _photoService.UploadAsync(dto.ImageFile,
               "categories");
                if (upload.IsFailure)
                    return Result<CategoryDTO>.Fail(upload.Errors!);
                cat.ImageURL = upload.Value;
            }

            _uow.GetRepository<Category>().Add(cat);
            await _uow.SaveChangesAsync();
            return await GetByIdAsync(cat.Id);
        }
        public async Task<Result<CategoryDTO>> UpdateAsync(int id, UpdateCategoryDTO dto)
        {
            var cat = await _uow.GetRepository<Category>().GetById(id);

            if (cat is null)
                return Result<CategoryDTO>.NotFound("Category", id);

            var existingByName = await _uow.GetRepository<Category>()
                .GetAll(c => c.Name == dto.Name && c.Id != id);
            if (existingByName.Any())
                return Result<CategoryDTO>.Fail(Error.Conflict("Category name already exists."));

            if (dto.RemoveExistingImage && cat.ImageURL != null)
            {
                await _photoService.DeleteAsync(cat.ImageURL); cat.ImageURL = null;
            }

            if (dto.ImageFile != null)
            {
                if (cat.ImageURL != null)
                    await _photoService.DeleteAsync(cat.ImageURL);

                var upload = await _photoService.UploadAsync(dto.ImageFile, "categories");
                if (upload.IsFailure) 
                    return Result<CategoryDTO>.Fail(upload.Errors!);
                cat.ImageURL = upload.Value;
            }

            cat.Name = dto.Name;
            cat.Description = dto.Description;
            cat.UpdatedAt = DateTime.UtcNow;
            _uow.GetRepository<Category>().Update(cat);

            await _uow.SaveChangesAsync();
            return await GetByIdAsync(id);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var cat = await _uow.GetRepository<Category>().GetById(id);
            if (cat is null) return Result.NotFound("Category", id);

            var hasProducts = (await _uow.GetRepository<Product>()
                .GetAll(p => p.CategoryId == id)).Any();
            if (hasProducts)
                return Result.Fail(Error.Conflict("Cannot delete category because it is referenced by products."));

            if (cat.ImageURL != null) await _photoService.DeleteAsync(cat.ImageURL);
            _uow.GetRepository<Category>().Delete(cat);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
    }
}