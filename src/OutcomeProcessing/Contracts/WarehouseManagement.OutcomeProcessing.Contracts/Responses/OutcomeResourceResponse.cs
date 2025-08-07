namespace WarehouseManagement.OutcomeProcessing.Contracts.Responses;

public record OutcomeResourceResponse(
    Guid Id, 
    OutcomeResourceResourceResponse Resource,
    OutcomeResourceUnitResponse Unit,
    int ResourceQuantity);

public record OutcomeResourceResourceResponse(
    Guid Id,
    string Title);

public record OutcomeResourceUnitResponse(
    Guid Id,
    string Title);