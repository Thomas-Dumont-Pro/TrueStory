using Application.Common.Models;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("MockApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["ConnectedServices:MockAPI:BaseUrl"] ?? throw new ConfigurationException("Invalid Mock API URL"));
        });
        services.AddScoped<IItemRepository, Repositories.ItemRepository>();
        return services;
    }
}