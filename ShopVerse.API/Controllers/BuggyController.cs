using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.API.Errors;
using ShopVerse.Repository.Data.Contexts;
using System.Threading.Tasks;

namespace ShopVerse.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly AppDbContext _context;

        public BuggyController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("notfound")]
        public async Task<IActionResult> GetNotFoundRequerstError()
        {
            var brand = await _context.Brands.FindAsync(42);
            if (brand is null)
                return NotFound(new ApiErrorResponse (404));
            return Ok(brand);
        }
        [HttpGet("servererror")]
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(42);
            var brandToString = brand.ToString(); // This will throw an exception (NullReferenceExeption)
            return Ok(brand);
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public IActionResult GetBadRequestError(int id)
        {
            return Ok();
        }
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorizedError(int id)
        {
            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}
