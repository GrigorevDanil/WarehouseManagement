namespace WarehouseManagement.ClientManagement.Contracts.Requests;

/// <summary>
/// Запрос на создание клиента
/// </summary>
/// <param name="Title">Название клиента</param>
/// <param name="Address">Адрес клиента</param>
public record CreateClientRequest(string Title, string Address);