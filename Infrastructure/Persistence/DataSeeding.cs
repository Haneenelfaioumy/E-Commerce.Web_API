using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext , 
                             UserManager<ApplicationUser> _userManager ,
                             RoleManager<IdentityRole> _roleManager ,
                             StoreIdentityDbContext _identityDbContext) : IDataSeeding
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

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Tarek",
                        PhoneNumber = "01558327535",
                        UserName = "MohamedTarek"
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        PhoneNumber = "01558327535",
                        UserName = "SalmaMohamed"
                    };
                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");

                    await _identityDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
