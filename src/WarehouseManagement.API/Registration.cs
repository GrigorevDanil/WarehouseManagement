using FluentValidation;
using Microsoft.OpenApi.Models;
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
        services
            .AddSwagger()
            .AddResourceModule(configuration)
            .AddClientModule(configuration)
            .AddUnitModule(configuration)
            .AddIncomeProcessingModule(configuration)
            .AddApplicationLayers();

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

    private static IServiceCollection AddApplicationLayers(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            typeof(WarehouseManagement.ResourceManagement.Application.Registration).Assembly,
            typeof(WarehouseManagement.ClientManagement.Application.Registration).Assembly,
            typeof(WarehouseManagement.UnitManagement.Application.Registration).Assembly,
            typeof(WarehouseManagement.IncomeProcessing.Application.Registration).Assembly,
        };

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
}