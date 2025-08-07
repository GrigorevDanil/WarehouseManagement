namespace WarehouseManagement.OutcomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на получение документов отгрузки с пагинацией
/// </summary>
/// <param name="Page">Страница</param>
/// <param name="PageSize">Количество документов отгрузки на странице</param>
/// <param name="StartDate">Дата начала диапазона</param>
/// <param name="EndDate">Дата конца диапазона</param>
/// <param name="NumDocument">Номер документа отгрузки </param>
/// <param name="ClientIds">Идентификаторы клиентов которые есть в документе</param>
/// <param name="ResourceIds">Идентификаторы ресурсов которые есть в документе</param>
/// <param name="UnitIds">Идентификаторы единиц измерения которые есть в документе</param>
/// <param name="SortDirection">Направление сортировки</param>
public record GetOutcomeDocumentsWithPaginationRequest(
    int Page,
    int PageSize,
    DateTime StartDate,
    DateTime EndDate,
    string? NumDocument,
    Guid?[] ClientIds,
    Guid?[] ResourceIds,
    Guid?[] UnitIds,
    string? SortDirection);