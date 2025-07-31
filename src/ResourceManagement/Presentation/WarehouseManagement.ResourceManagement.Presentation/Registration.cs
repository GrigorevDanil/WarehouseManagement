using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.ResourceManagement.Contracts;

namespace WarehouseManagement.ResourceManagement.Presentation;

public static class Registration
{
    public static IServiceCollection AddResourceManagementPresentation(this IServiceCollection services)
    {
        services.AddScoped<IResourceManagementContract, ResourceManagementContract>();

        return services;
    }
}