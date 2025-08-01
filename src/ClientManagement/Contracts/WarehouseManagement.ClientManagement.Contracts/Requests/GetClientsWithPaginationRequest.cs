namespace WarehouseManagement.ClientManagement.Contracts.Requests;

/// <summary>
/// Запрос на получение клиентов с пагинацией
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество клиентов на странице</param>
/// <param name="Title">Название клиента</param>
/// <param name="SortDirection">Направление сортировки</param>
/// <param name="IsArchive">Клиенты из архива</param>
public record GetClientsWithPaginationRequest(
    int Page,
    int PageSize,
    string? Title,
    string? SortDirection,
    bool? IsArchive);