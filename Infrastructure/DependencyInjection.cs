﻿using Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<ItemRepository, Repositories.ItemRepository>();
        return services;
    }
}