using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.ClientManagement.Contracts;

namespace WarehouseManagement.ClientManagement.Presentation;

public static class Registration
{
    public static IServiceCollection AddClientManagementPresentation(this IServiceCollection services)
    {
        services.AddScoped<IClientManagementContract, ClientManagementContract>();

        return services;
    }
}