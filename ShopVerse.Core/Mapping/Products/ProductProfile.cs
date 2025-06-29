using AutoMapper;
using Microsoft.Extensions.Configuration;
using ShopVerse.Core.Dtos.Product;
using ShopVerse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d=>d.BrandName,options=>options.MapFrom(s=>s.Brand.Name))
                .ForMember(d=>d.TypeName,options=>options.MapFrom(s=>s.Type.Name))
                .ForMember(p => p.PictureUrl, options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.PictureUrl}"));
            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
