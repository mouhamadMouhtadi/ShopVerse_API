using Microsoft.EntityFrameworkCore;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Entities.Order;
using ShopVerse.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopVerse.Repository.Data
{
    public class StoreDbContextSeed
    {
        public async static Task SeedAsync(AppDbContext _context)
        {
            if(_context.Brands.Count() == 0)
            {
                // brand 
                //1 read data from json file
                var brandsData = File.ReadAllText(@"..\ShopVerse.Repository\Data\DataSeed\brands.json");
                //2 convert json to list<T>

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                //3 seed data to database
                if (brands is not null && brands.Count() > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }
            if (_context.Types.Count() == 0)
            {
                // brand 
                //1 read data from json file
                var typesData = File.ReadAllText(@"..\ShopVerse.Repository\Data\DataSeed\types.json");
                //2 convert json to list<T>

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                //3 seed data to database
                if (types is not null && types.Count() > 0)
                {
                    await _context.Types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }
            if (_context.Products.Count() == 0)
            {
                // brand 
                //1 read data from json file
                var ProductsData = File.ReadAllText(@"..\ShopVerse.Repository\Data\DataSeed\products.json");
                //2 convert json to list<T>

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                //3 seed data to database
                if (products is not null && products.Count() > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.DeliveryMethods.Count() == 0)
            {
                // brand 
                //1 read data from json file
                var DeliveryData = File.ReadAllText(@"..\ShopVerse.Repository\Data\DataSeed\delivery.json");
                //2 convert json to list<T>

                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                //3 seed data to database
                if (DeliveryMethods is not null)
                {
                    await _context.DeliveryMethods.AddRangeAsync(DeliveryMethods);
                    await _context.SaveChangesAsync();
                }
            }

        }
    }
}
