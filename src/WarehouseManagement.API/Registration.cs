using System.Reflection;
using MassTransit;
using FluentValidation;
using Microsoft.OpenApi.Models;
using WarehouseManagement.BalanceManagement.Infrastructure;
using WarehouseManagement.BalanceManagement.Presentation;
using WarehouseManagement.ClientManagement.Infrastructure;
using WarehouseManagement.ClientManagement.Presentation;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Infrastructure;
using WarehouseManagement.IncomeProcessing.Presentation;
using WarehouseManagement.ResourceManagement.Infrastructure;
using WarehouseManagement.ResourceManagement.Presentation;
using WarehouseManagement.UnitManagement.Infrastructure;
using WarehouseManagement.UnitManagement.Presentation;

namespace WarehouseManagement.API;

public static class Registration
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = new[]
        {
            typeof(WarehouseManagement.ResourceManagement.Application.Registration).Assembly,
            typeof(WarehouseManagement.ClientManagement.Application.Registration).Assembly,
            typeof(WarehouseManagement.UnitManagement.Application.Registration).Assembly,
            typeof(WarehouseManagement.IncomeProcessing.Application.Registration).Assembly,
            typeof(WarehouseManagement.BalanceManagement.Application.Registration).Assembly,
        };
        
        services
            .AddSwagger()
            .AddResourceModule(configuration)
            .AddClientModule(configuration)
            .AddUnitModule(configuration)
            .AddIncomeProcessingModule(configuration)
            .AddBalanceModule(configuration)
            .AddApplicationLayers(assemblies)
            .AddMessageBus(configuration, assemblies);
        

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Warehouse Management API",
                Version = "v1"
            });

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            }
        });

        return services;
    }

    private static IServiceCollection AddResourceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddResourceManagementPresentation()
            .AddResourceManagementInfrastructure(configuration);

        return services;
    }
    
    private static IServiceCollection AddClientModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddClientManagementPresentation()
            .AddClientManagementInfrastructure(configuration);

        return services;
    }
    
    private static IServiceCollection AddUnitModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddUnitManagementPresentation()
            .AddUnitManagementInfrastructure(configuration);

        return services;
    }
    
    private static IServiceCollection AddIncomeProcessingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIncomeProcessingPresentation()
            .AddIncomeProcessingInfrastructure(configuration);

        return services;
    }
    
    private static IServiceCollection AddBalanceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddBalanceManagementPresentation()
            .AddBalanceManagementInfrastructure(configuration);

        return services;
    }

    private static IServiceCollection AddApplicationLayers(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<>), typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.AddValidatorsFromAssemblies(assemblies);
        
        return services;
    }
    
    private static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration, 
        Assembly[] assemblies)
    {
        services.AddMassTransit(configure =>
        {
            foreach (var assembly in assemblies) 
                configure.AddConsumers(assembly);

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMQ:Host"]!), h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.Durable = true;
                
                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

