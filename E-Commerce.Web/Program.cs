using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using Persistence;
using Service;


namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();

            #endregion

            var app = builder.Build();

            #region Data Seeding.

            await app.SeedDataBaseAsync();

            #endregion

            #region Configure the HTTP request pipeline.

            app.UseCustomExceptionMiddleWare();
            if (app.Environment.IsDevelopment())
            {
               app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseAuthorization();

            app.MapControllers(); 

            #endregion

            app.Run();
        }
    }
}
