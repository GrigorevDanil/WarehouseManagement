using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.OutcomeProcessing.Contracts;

namespace WarehouseManagement.OutcomeProcessing.Presentation;

public static class Registration
{
    public static IServiceCollection AddOutcomeProcessingPresentation(this IServiceCollection services)
    {
        services.AddScoped<IOutcomeProcessingContract, OutcomeProcessingContract>();

        return services;
    }
}