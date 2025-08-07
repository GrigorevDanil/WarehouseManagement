namespace WarehouseManagement.IncomeProcessing.Contracts.Requests;

public record AddIncomeResourceRequest(
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity);