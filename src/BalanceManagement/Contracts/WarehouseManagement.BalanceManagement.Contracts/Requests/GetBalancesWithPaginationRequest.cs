namespace WarehouseManagement.BalanceManagement.Contracts.Requests;

/// <summary>
/// Запрос на получение балансов с пагинацией
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество балансов на странице</param>
/// <param name="ResourceIds">Идентификаторы ресурсов которые есть в балансе</param>
/// <param name="UnitIds">Идентификаторы единиц измерения которые есть в балансе</param>
/// <param name="SortDirection">Направление сортировки</param>
public record GetBalancesWithPaginationRequest(
    int Page,
    int PageSize,
    Guid?[] ResourceIds,
    Guid?[] UnitIds,
    string? SortDirection);