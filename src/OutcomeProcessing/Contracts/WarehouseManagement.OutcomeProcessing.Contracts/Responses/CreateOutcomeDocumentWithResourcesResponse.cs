namespace WarehouseManagement.OutcomeProcessing.Contracts.Responses;

public record CreateOutcomeDocumentWithResourcesResponse(
    Guid OutcomeDocumentId,
    Guid[] OutcomeResourceIds);