namespace WarehouseManagement.ResourceManagement.Contracts.Requests;

/// <summary>
/// Запрос на получение списка ресурсов с пагинацией
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество ресурсов на странице</param>
/// <param name="Title">Название ресурса</param>
/// <param name="SortDirection">Направление сортировки</param>
/// <param name="IsArchive">Ресурсы из архива</param>
public record GetResourcesWithPaginationRequest(
    int Page,
    int PageSize,
    string? Title,
    string? SortDirection,
    bool? IsArchive);