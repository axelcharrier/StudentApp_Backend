namespace StudentApp.Infrastructure.Extension;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentApp.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// We configured the service with the dbContext
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<myDbContext>(options =>
        options
            .UseSqlServer(
                configuration.GetConnectionString("DefaultConnexion"),
                x =>
                {
                    x.MigrationsAssembly(typeof(myDbContext).Assembly.GetName().Name);
                }
            )
         );

        return services;
    }
}
