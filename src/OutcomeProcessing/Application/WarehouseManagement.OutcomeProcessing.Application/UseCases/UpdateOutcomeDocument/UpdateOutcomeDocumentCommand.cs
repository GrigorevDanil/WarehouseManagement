using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.OutcomeProcessing.Contracts.Requests;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeDocument;

public record UpdateOutcomeDocumentCommand(
    Guid OutcomeDocumentId,
    string NumDocument,
    Guid ClientId,
    DateTime? CreatedAt) : ICommand
{
    public static UpdateOutcomeDocumentCommand Create(Guid outcomeDocumentId, UpdateOutcomeDocumentRequest request) => 
        new(outcomeDocumentId, request.NumDocument, request.ClientId, request.CreatedAt);
}