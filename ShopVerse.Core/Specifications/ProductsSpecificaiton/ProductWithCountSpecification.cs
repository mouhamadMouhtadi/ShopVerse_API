using ShopVerse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Specifications.ProductsSpecificaiton
{
    public class ProductWithCountSpecification: BaseSpecifications<Product, int>
    {

        public ProductWithCountSpecification(ProductSpecParams productSpec) : base(
            p =>
             (string.IsNullOrEmpty(productSpec.Search) || p.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.brandId.HasValue || productSpec.brandId == p.BrandId)
            &&
            (!productSpec.typeId.HasValue || productSpec.typeId == p.TypeId))
        {
         
        }
        
    }
}
