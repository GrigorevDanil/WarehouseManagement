using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.OutcomeProcessing.Contracts.Requests;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.AddOutcomeResource;

public record AddOutcomeResourceCommand(
    Guid OutcomeDocumentId,
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity) : ICommand
{
    public static AddOutcomeResourceCommand Create(Guid outcomeDocumentId, AddOutcomeResourceRequest request) => 
        new(outcomeDocumentId, request.ResourceId, request.UnitId, request.ResourceQuantity);
}