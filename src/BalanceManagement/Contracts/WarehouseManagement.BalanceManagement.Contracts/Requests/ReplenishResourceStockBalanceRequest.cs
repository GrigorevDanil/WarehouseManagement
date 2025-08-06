namespace WarehouseManagement.BalanceManagement.Contracts.Requests;

/// <summary>
/// Запрос на пополнение количества ресурсов
/// </summary>
/// <param name="AddedResourceStock">Добавляемое количество ресурсов</param>
public record ReplenishResourceStockBalanceRequest(int AddedResourceStock);