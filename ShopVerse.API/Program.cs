
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
          builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile(builder.Configuration)));
            var app = builder.Build();



           using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            var loggerfactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                               // Ensure the database is created and apply migrations if necessary
               await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.ToString());
            }
                if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
