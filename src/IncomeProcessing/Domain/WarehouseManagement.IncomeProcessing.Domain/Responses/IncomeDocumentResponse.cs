namespace WarehouseManagement.IncomeProcessing.Domain.Responses;

public record IncomeDocumentResponse(
    Guid Id,
    string NumDocument,
    DateTime CreatedAt,
    IncomeResourceResponse[] Resources);