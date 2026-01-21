namespace StudentApp.Application.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentApp.Infrastructure.Extension;

/// <summary>
/// Provides extension methods for registering application services.
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Adds application-level services to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to which the application services will be added.</param>
    /// <param name="configuration">The configuration settings used to configure the application services.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        return services;
    }
}
