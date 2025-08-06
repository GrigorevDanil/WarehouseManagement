namespace WarehouseManagement.BalanceManagement.Contracts.Requests;

/// <summary>
/// Запрос на создание баланса
/// </summary>
/// <param name="ResourceId">Идентификатор ресурса</param>
/// <param name="UnitId">Идентификатор единицы измерения</param>
/// <param name="ResourceStock">Количество ресурсов</param>
public record CreateBalanceRequest(Guid ResourceId, Guid UnitId, int ResourceStock);