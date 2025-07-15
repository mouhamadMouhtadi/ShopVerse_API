using Microsoft.AspNetCore.Mvc.Filters;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Services.Services.Cashes;
using System.Text;

namespace ShopVerse.API.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetService<ICasheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCasheKeyAsync(cacheKey);
            if(!string.IsNullOrEmpty(cacheResponse))
            {
               var contextResult = new Microsoft.AspNetCore.Mvc.ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contextResult;
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is Microsoft.AspNetCore.Mvc.ObjectResult response)
            {
                    await cacheService.SetCasheKeyAsync(cacheKey, response.Value , TimeSpan.FromSeconds(_expireTime));
            }
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(X => X.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }
            return cacheKey.ToString();
        }
    }
}
