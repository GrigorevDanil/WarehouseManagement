using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using WarehouseManagement.BalanceManagement.Application.Interfaces;
using WarehouseManagement.BalanceManagement.Domain.Entities;
using WarehouseManagement.BalanceManagement.Infrastructure.DbContexts;
using WarehouseManagement.BalanceManagement.Infrastructure.Repositories;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Framework.Hosting;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.BalanceManagement.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddBalanceManagementInfrastructure(this IServiceCollection services,
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
        
        services.AddScoped<IBalanceReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        services.AddTransient<IStartupFilter, DbContextMigrationFilter<WriteDbContext>>();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Balance,BalanceId>, BalanceRepository>();
        services.AddScoped<IBalanceRepository, BalanceRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.BalanceManagement);
        
        return services;
    }
}