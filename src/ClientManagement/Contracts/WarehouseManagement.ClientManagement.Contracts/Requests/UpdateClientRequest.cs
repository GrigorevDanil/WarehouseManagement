namespace WarehouseManagement.ClientManagement.Contracts.Requests;

/// <summary>
/// Запрос на обновление клиента
/// </summary>
/// <param name="Title">Название клиента</param>
/// <param name="Address">Адрес клиента</param>
public record UpdateClientRequest(string Title, string Address);