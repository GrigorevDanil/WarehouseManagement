namespace WarehouseManagement.UnitManagement.Contracts.Requests;

/// <summary>
/// Запрос на получение списка единиц измерений с пагинацией
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество единиц измерений на странице</param>
/// <param name="Title">Название единицы измерения</param>
/// <param name="SortDirection">Направление сортировки</param>
/// <param name="IsArchive">Единицы измерения из архива</param>
public record GetUnitsWithPaginationsRequest(
    int Page,
    int PageSize,
    string? Title,
    string? SortDirection,
    bool? IsArchive);