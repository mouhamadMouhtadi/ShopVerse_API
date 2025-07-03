
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopVerse.API.Errors;
using ShopVerse.API.Helper;
using ShopVerse.API.MiddleWare;
using ShopVerse.Core;
using ShopVerse.Core.Mapping.Products;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Repository;
using ShopVerse.Repository.Data;
using ShopVerse.Repository.Data.Contexts;
using ShopVerse.Services.Services.Products;
using System.Threading.Tasks;

namespace ShopVerse.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDependency(builder.Configuration);


            var app = builder.Build();

           await app.ConfigureMiddleWaresAsync();
            app.Run();
        }
    }
}
