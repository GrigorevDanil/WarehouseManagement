using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Contracts;
using WarehouseManagement.ResourceManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Presentation;

public static class Registration
{
    public static IServiceCollection AddIncomeProcessingPresentation(this IServiceCollection services)
    {
        services.AddScoped<IIncomeProcessingContract, IncomeProcessingContract>();

        return services;
    }
}