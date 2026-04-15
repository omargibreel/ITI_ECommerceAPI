using Common.Result.Ecommerce.BLL.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<string>> UploadAsync(IFormFile file, string folder);
        Task<Result> DeleteAsync(string relativePath);
    }
}
