namespace WarehouseManagement.ResourceManagement.Contracts.Requests;

/// <summary>
/// Запрос на создание ресурса
/// </summary>
/// <param name="Title">Название ресурса</param>
public record CreateResourceRequest(string Title);