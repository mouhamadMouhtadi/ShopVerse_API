using ShopVerse.Core.Dtos.Product;
using ShopVerse.Core.Helper;
using ShopVerse.Core.Specifications.ProductsSpecificaiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Services.Interfaces
{
    public interface IProductService
    {
       Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec);
        Task<ProductDto> GetProductByIdAsync(int id);
       Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
       Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
    }
}
