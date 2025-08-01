using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.UnitManagement.Presentation;

public static class Registrations
{
    public static IServiceCollection AddUnitManagementPresentation(this IServiceCollection services)
    {
        services.AddScoped<IUnitManagementContract, UnitManagementContract>();

        return services;
    }
}