namespace WarehouseManagement.BalanceManagement.Contracts.Requests;

/// <summary>
/// Запрос на уменьшение количества ресурсов
/// </summary>
/// <param name="SubtractedResourceStock">Вычитаемое количество ресурсов</param>
public record ReduceResourceStockBalanceRequest(int SubtractedResourceStock);