namespace WarehouseManagement.IncomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на обновление ресурса в документе поступления
/// </summary>
/// <param name="ResourceId">Идентификатор ресурса</param>
/// <param name="UnitId">Идентификатор единицы измерения</param>
/// <param name="ResourceStock">Количество ресурсов</param>
public record UpdateIncomeResourceRequest(Guid ResourceId, Guid UnitId, int ResourceStock);