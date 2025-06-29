using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.Core;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Core.Specifications.ProductsSpecificaiton;
using System.Threading.Tasks;

namespace ShopVerse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductSpecParams productSpec)
        {
            var result = await _productService.GetAllProductsAsync(productSpec);
            return Ok(result);
        }
        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("Product ID cannot be null.");
            var result = await _productService.GetProductByIdAsync(id.Value);
            if (result is null) return NotFound($"Product with ID {id} not found.");
            return Ok(result);
        }
    }
}
