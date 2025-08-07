using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.OutcomeProcessing.Contracts.Requests;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.CreateOutcomeDocumentWithResources;

public record CreateOutcomeDocumentWithResourcesCommand(
    string NumDocument, 
    Guid ClientId, 
    DateTime? CreatedAt, 
    CreateOutcomeDocumentWithResourcesCommandItem[] Items) : ICommand
{
    public static CreateOutcomeDocumentWithResourcesCommand Create(CreateOutcomeDocumentWithResourcesRequest request)
    {
        return new CreateOutcomeDocumentWithResourcesCommand(
            request.NumDocument,
            request.ClientId,
            request.CreatedAt,
            request.Items.Select(item => new CreateOutcomeDocumentWithResourcesCommandItem(
                item.ResourceId,
                item.UnitId,
                item.ResourceQuantity)).ToArray());
    }
}

public record CreateOutcomeDocumentWithResourcesCommandItem(
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity);