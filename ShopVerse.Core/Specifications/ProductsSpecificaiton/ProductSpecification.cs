using ShopVerse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Specifications.ProductsSpecificaiton
{
    public class ProductSpecification :BaseSpecifications<Product,int>
    {
        public ProductSpecification(int id) : base(P=>P.Id == id)
        {
            ApplyIncludes();
        }
        public ProductSpecification(ProductSpecParams productSpec) :base(
            p=>
            (string.IsNullOrEmpty(productSpec.Search) || p.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.brandId.HasValue || productSpec.brandId == p.BrandId)
            &&
            (!productSpec.typeId.HasValue || productSpec.typeId == p.TypeId))
        {
            if (!string.IsNullOrEmpty(productSpec.sort))
            {
                switch (productSpec.sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }else
            {
                AddOrderBy(P => P.Name);
            }
                ApplyIncludes();
            ApplyPagination(productSpec.pageSize * (productSpec.pageIndex - 1), productSpec.pageSize);
        }
        private void ApplyIncludes()
        {
            Includes.Add(P=>P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
