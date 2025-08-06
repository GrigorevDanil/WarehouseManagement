using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Framework.Outbox;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.IncomeProcessing.Infrastructure.DbContexts;
using WarehouseManagement.IncomeProcessing.Infrastructure.Outbox;
using WarehouseManagement.IncomeProcessing.Infrastructure.Repositories;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddIncomeProcessingInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddServices()
            .AddQuartzServices();
        
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        services.AddScoped<IIncomeProcessingReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<IncomeDocument, IncomeDocumentId>, IncomeDocumentRepository>();
        
        services.AddKeyedScoped<IOutboxRepository, OutboxRepository>(Modules.IncomeProcessing);
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.IncomeProcessing);

        return services;
    }
    
    private static IServiceCollection AddQuartzServices(this IServiceCollection services)
    {
        services.AddScoped<IOutboxMessageProcess, OutboxMessageProcess<WriteDbContext>>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(OutboxMessageProcessJob));

            configure
                .AddJob<OutboxMessageProcessJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(1).RepeatForever()));
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
    
}