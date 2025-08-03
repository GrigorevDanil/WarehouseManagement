namespace WarehouseManagement.IncomeProcessing.Domain.Responses;

public record IncomeResourceResponse(
    Guid Id, 
    ResourceResponse Resource,
    UnitResponse Unit,
    int ResourceStock);

public record ResourceResponse(
    Guid Id,
    string Title);

public record UnitResponse(
    Guid Id,
    string Title);