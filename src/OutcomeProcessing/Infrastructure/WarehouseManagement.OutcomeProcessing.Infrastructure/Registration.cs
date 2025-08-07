using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Framework.Outbox;
using WarehouseManagement.OutcomeProcessing.Application.Interfaces;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Infrastructure.DbContexts;
using WarehouseManagement.OutcomeProcessing.Infrastructure.Outbox;
using WarehouseManagement.OutcomeProcessing.Infrastructure.Repositories;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddOutcomeProcessingInfrastructure(this IServiceCollection services,
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
        
        services.AddScoped<IOutcomeProcessingReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE_KEY)!));
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<OutcomeDocument, OutcomeDocumentId>, OutcomeDocumentRepository>();
        
        services.AddKeyedScoped<IOutboxRepository, OutboxRepository>(Modules.OutcomeProcessing);
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.OutcomeProcessing);

        return services;
    }
    private static IServiceCollection AddQuartzServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IOutboxMessageProcess, OutboxMessageProcess<WriteDbContext>>(Modules.OutcomeProcessing);

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(Modules.OutcomeProcessing) + nameof(OutboxMessageProcessJob));

            configure
                .AddJob<OutboxMessageProcessJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
    
}