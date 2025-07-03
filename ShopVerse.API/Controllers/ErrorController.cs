using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.API.Errors;

namespace ShopVerse.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code) {
        return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
        }
    }
}
