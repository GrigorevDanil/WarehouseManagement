namespace WarehouseManagement.OutcomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на создание ресурса отгрузки
/// </summary>
/// <param name="ResourceId">Идентификатор ресурса</param>
/// <param name="UnitId">Идентификатор единицы измерения</param>
/// <param name="ResourceQuantity">Количество ресурсов</param>
public record AddOutcomeResourceRequest(
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity);