using Common.Result.Ecommerce.BLL.Results;
using Ecommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Services.Classes
{
    public class PhotoService : IPhotoService
    {
        public Task<Result<string>> UploadAsync(IFormFile file, string folder)
        {
            throw new NotImplementedException();
        }
        public Task<Result> DeleteAsync(string relativePath)
        {
            throw new NotImplementedException();
        }
    }
}
