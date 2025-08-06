namespace WarehouseManagement.IncomeProcessing.Contracts.Responses;

public record IncomeResourceResponse(
    Guid Id, 
    IncomeResourceResourceResponse Resource,
    IncomeResourceUnitResponse Unit,
    int ResourceStock);

public record IncomeResourceResourceResponse(
    Guid Id,
    string Title);

public record IncomeResourceUnitResponse(
    Guid Id,
    string Title);