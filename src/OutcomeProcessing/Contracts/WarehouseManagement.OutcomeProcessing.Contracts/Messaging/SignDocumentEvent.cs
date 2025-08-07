namespace WarehouseManagement.OutcomeProcessing.Contracts.Messaging;

public record SignDocumentEvent(
    Guid ResourceId,
    Guid UnitId,
    int SubtractedResourceQuantity);