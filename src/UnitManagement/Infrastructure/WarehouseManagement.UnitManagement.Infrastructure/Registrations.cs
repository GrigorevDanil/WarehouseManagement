using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Framework.Hosting;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Application.Interfaces;
using WarehouseManagement.UnitManagement.Domain.Entities;
using WarehouseManagement.UnitManagement.Infrastructure.DbContexts;
using WarehouseManagement.UnitManagement.Infrastructure.Repositories;

namespace WarehouseManagement.UnitManagement.Infrastructure;

public static class Registrations
{
    public static IServiceCollection AddUnitManagementInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddServices();
        
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        services.AddScoped<IUnitReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        services.AddTransient<IStartupFilter, DbContextMigrationFilter<WriteDbContext>>();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Unit, UnitId>, UnitRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.UnitManagement);
        
        return services;
    }
}