namespace WarehouseManagement.BalanceManagement.Contracts.Responses;

public record BalanceResponse(
    Guid Id, 
    BalanceResourceResponse Resource,
    BalanceUnitResponse Unit,
    int ResourceStock);

public record BalanceResourceResponse(
    Guid Id,
    string Title);

public record BalanceUnitResponse(
    Guid Id,
    string Title);