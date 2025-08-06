using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.BalanceManagement.Contracts;

namespace WarehouseManagement.BalanceManagement.Presentation;

public static class Registration
{
    public static IServiceCollection AddBalanceManagementPresentation(this IServiceCollection services)
    {
        services.AddScoped<IBalanceManagementContract, BalanceManagementContract>();

        return services;
    }
}