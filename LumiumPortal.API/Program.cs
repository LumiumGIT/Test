using Lumium.Application.Common.Interfaces;
using Lumium.Infrastructure.Middleware;
using Lumium.Infrastructure.MultiTenancy;
using Lumium.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MasterDatabase")));

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var tenantContext = serviceProvider.GetRequiredService<ITenantContext>();
    
    // Ovde ćeš kasnije dodati logiku za per-tenant connection string
    // Za sada koristi master connection
    options.UseNpgsql(builder.Configuration.GetConnectionString("MasterDatabase"));
});

builder.Services.AddScoped<ITenantContext, TenantContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<TenantResolutionMiddleware>();

app.UseHttpsRedirection();

app.Run();