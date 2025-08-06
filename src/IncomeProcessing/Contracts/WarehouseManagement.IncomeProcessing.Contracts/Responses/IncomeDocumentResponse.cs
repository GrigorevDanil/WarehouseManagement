namespace WarehouseManagement.IncomeProcessing.Contracts.Responses;

public record IncomeDocumentResponse(
    Guid Id,
    string NumDocument,
    DateTime CreatedAt,
    IncomeResourceResponse[] Resources);