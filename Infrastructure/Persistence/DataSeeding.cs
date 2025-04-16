using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                // Production
                if (PendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    // Read Data
                    //var ProductBrandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrandData =  File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    // Convert Data "String" => C# Objects [ProductBrand]
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);
                    // Save To DB
                    if (ProductBrands is not null && ProductBrands.Any())
                        await _dbContext.ProductBrands.AddRangeAsync(ProductBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    // Read Data
                    var ProductTypeData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    // Convert Data "String" => C# Objects [ProductType]
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypeData);
                    // Save To DB
                    if (ProductTypes is not null && ProductTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(ProductTypes);
                }
                if (!_dbContext.Products.Any())
                {
                    // Read Data
                    var ProductData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    // Convert Data "String" => C# Objects [ProductType]
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);
                    // Save To DB
                    if (Products is not null && Products.Any())
                        await _dbContext.Products.AddRangeAsync(Products);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO => Go To Error Page
            }
        }
    }
}
