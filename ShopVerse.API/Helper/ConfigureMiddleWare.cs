using Microsoft.EntityFrameworkCore;
using ShopVerse.API.MiddleWare;
using ShopVerse.Repository.Data;
using ShopVerse.Repository.Data.Contexts;

namespace ShopVerse.API.Helper
{
    public static class ConfigureMiddleWare
    {
        public static async Task<WebApplication> ConfigureMiddleWaresAsync(this WebApplication app)
        {
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

            app.UseMiddleware<ExceptionMiddleWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            return app;

        }
    }
}
