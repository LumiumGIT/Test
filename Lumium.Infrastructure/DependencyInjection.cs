using Lumium.Application.Common.Interfaces;
using Lumium.Infrastructure.Interceptors;
using Lumium.Infrastructure.MultiTenancy;
using Lumium.Infrastructure.Persistence;
using Lumium.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lumium.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ITenantContext, TenantContext>();
        services.AddScoped<TenantConnectionInterceptor>();

        services.AddDbContext<MasterDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("MasterDatabase")));

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ApplicationDatabase"))
                .AddInterceptors(serviceProvider.GetRequiredService<TenantConnectionInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}