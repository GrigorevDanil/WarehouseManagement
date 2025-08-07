using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.OutcomeProcessing.Contracts.Requests;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeResource;

public record UpdateOutcomeResourceCommand(
    Guid OutcomeDocumentId,
    Guid OutcomeResourceId,
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity) : ICommand
{
    public static UpdateOutcomeResourceCommand Create(Guid outcomeDocumentId, Guid outcomeResourceId, UpdateOutcomeResourceRequest request) => 
        new(outcomeDocumentId, outcomeResourceId, request.ResourceId, request.UnitId, request.ResourceQuantity);
}