using AutoMapper;
using ShopVerse.Core;
using ShopVerse.Core.Dtos.Product;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Helper;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Core.Specifications.ProductsSpecificaiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Services.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification( productSpec);
            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
           var mappedproduct= _mapper.Map<IEnumerable<ProductDto>>(products);
            var countSpec = new ProductWithCountSpecification(productSpec);
            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);
            return new PaginationResponse<ProductDto>(productSpec.pageSize, productSpec.pageIndex, count, mappedproduct);
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
          => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
    
        public async Task<ProductDto> GetProductByIdAsync(int id)
         {
            var spec = new ProductSpecification(id);
            var product =  await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);
          var mappedProduct =   _mapper.Map<ProductDto>(product);
            return mappedProduct;
        }
        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
         =>   _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());


    }
}
