namespace WarehouseManagement.IncomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на получение документов поступления с пагинацией
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество документов поступления на странице</param>
/// <param name="StartDate">Дата начала диапазона</param>
/// <param name="EndDate">Дата конца диапазона</param>
/// <param name="NumDocument">Номер документа поступления </param>
/// <param name="ResourceIds">Идентификаторы ресурсов которые есть в документе</param>
/// <param name="UnitIds">Идентификаторы единиц измерения которые есть в документе</param>
/// <param name="SortDirection">Направление сортировки</param>
public record GetIncomeDocumentsWithPaginationRequest(
    int Page,
    int PageSize,
    DateTime StartDate,
    DateTime EndDate,
    string? NumDocument,
    Guid?[] ResourceIds,
    Guid?[] UnitIds,
    string? SortDirection);