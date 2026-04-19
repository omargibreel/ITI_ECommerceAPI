using Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result)
            => result.IsSuccess ? new OkResult() : result.Errors!.Any(e => e.Code.Contains("NotFound")) ? new
       NotFoundObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Validation")) ? new
       BadRequestObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Conflict")) ? new
       ConflictObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Unauthorized")) ? new
       UnauthorizedObjectResult(result) : new ObjectResult(result) { StatusCode = 500 };
        public static IActionResult ToActionResult<T>(this Result<T> result) where T : class
            => result.IsSuccess ? new OkObjectResult(result.Value) : result.Errors!.Any(e => e.Code.Contains("NotFound")) ? new
       NotFoundObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Validation")) ? new
       BadRequestObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Unauthorized")) ? new
       BadRequestObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Conflict")) ? new
       ConflictObjectResult(result) : result.Errors!.Any(e => e.Code.Contains("Unauthorized")) ? new
       UnauthorizedObjectResult(result) : new ObjectResult(result) { StatusCode = 500 };
    }
}
