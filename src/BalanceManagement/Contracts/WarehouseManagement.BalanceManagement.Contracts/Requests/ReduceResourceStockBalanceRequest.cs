namespace WarehouseManagement.BalanceManagement.Contracts.Requests;

/// <summary>
/// Запрос на уменьшение количества ресурсов
/// </summary>
/// <param name="SubtractedResourceQuantity">Вычитаемое количество ресурсов</param>
public record ReduceResourceStockBalanceRequest(int SubtractedResourceQuantity);