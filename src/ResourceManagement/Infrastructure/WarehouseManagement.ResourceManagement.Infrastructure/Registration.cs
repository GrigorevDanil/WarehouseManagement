using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.ResourceManagement.Infrastructure.DbContexts;
using WarehouseManagement.ResourceManagement.Infrastructure.Repositories;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddResourceManagementInfrastructure(this IServiceCollection services,
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
        
        services.AddScoped<IResourceReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Resource, ResourceId>, ResourceRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.ResourceManagement);
        
        return services;
    }

}