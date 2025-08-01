using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.ClientManagement.Application.Interfaces;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.ClientManagement.Infrastructure.DbContexts;
using WarehouseManagement.ClientManagement.Infrastructure.Repositories;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ClientManagement.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddClientManagementInfrastructure(this IServiceCollection services,
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
        
        services.AddScoped<IClientReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Client, ClientId>,  ClientRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.ClientManagement);
        
        return services;
    }
}