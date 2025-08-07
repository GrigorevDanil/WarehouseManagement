namespace WarehouseManagement.OutcomeProcessing.Contracts.Responses;

public record OutcomeDocumentResponse(
    Guid Id,
    string NumDocument,
    OutcomeDocumentClientResponse Client,
    DateTime CreatedAt,
    string Status,
    OutcomeResourceResponse[] Resources);
    
public record OutcomeDocumentClientResponse(
    Guid Id,
    string Title);