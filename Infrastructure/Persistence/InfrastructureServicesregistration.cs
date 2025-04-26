
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public static class InfrastructureServicesregistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services,
                                                                   IConfiguration Configuration)
        {
            Services.AddDbContext<StoreDbContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddScoped<IDataSeeding, DataSeeding>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            return Services;
        }
    }
}
