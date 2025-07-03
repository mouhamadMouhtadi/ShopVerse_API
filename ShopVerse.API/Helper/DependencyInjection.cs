using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopVerse.API.Errors;
using ShopVerse.Core;
using ShopVerse.Core.Mapping.Baskets;
using ShopVerse.Core.Mapping.Products;
using ShopVerse.Core.Respository.Interfaces;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Repository;
using ShopVerse.Repository.Data.Contexts;
using ShopVerse.Repository.Repository;
using ShopVerse.Services.Services.Products;
using StackExchange.Redis;

namespace ShopVerse.API.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwagerService();
            services.AddDbContextService(configuration);
            services.AddUserDefinedService();
            services.AddAutoMapperService(configuration);
            services.ConfigureInvalidModelStateResponseService();
            services.AddRedisService(configuration);
            return services;
        }
        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddSwagerService(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection AddDbContextService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                         options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            return services;
        }
        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            return services;
        }
        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }
        private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            return services;
        }
    }
}
