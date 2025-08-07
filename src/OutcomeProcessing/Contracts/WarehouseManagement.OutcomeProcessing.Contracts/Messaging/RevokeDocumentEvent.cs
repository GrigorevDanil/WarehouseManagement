namespace WarehouseManagement.OutcomeProcessing.Contracts.Messaging;

public record RevokeDocumentEvent(
    Guid ResourceId,
    Guid UnitId,
    int AddedResourceStock);