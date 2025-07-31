namespace WarehouseManagement.ResourceManagement.Contracts.Requests;

/// <summary>
/// Запрос на обновление ресурса
/// </summary>
/// <param name="Title">Название ресурса</param>
public record UpdateResourceRequest(string Title);