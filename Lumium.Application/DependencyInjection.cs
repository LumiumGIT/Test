using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Lumium.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);
        
        services.AddAutoMapper(_ => { }, assembly);

        return services;
    }
}