namespace WarehouseManagement.OutcomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на обновление ресурса отгрузки
/// </summary>
/// <param name="ResourceId">Идентификатор ресурса</param>
/// <param name="UnitId">Идентификатор единицы измерения</param>
/// <param name="ResourceQuantity">Количество ресурсов</param>
public record UpdateOutcomeResourceRequest(Guid ResourceId, Guid UnitId, int ResourceQuantity);