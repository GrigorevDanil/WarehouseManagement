namespace WarehouseManagement.OutcomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на создание документа отгрузки с ресурсами
/// </summary>
/// <param name="NumDocument">Номер документа</param>
/// <param name="ClientId">Идентификатор клиента</param>
/// <param name="CreatedAt">Создан в</param>
/// <param name="Items">Ресурсы</param>
public record CreateOutcomeDocumentWithResourcesRequest(
    string NumDocument,
    Guid ClientId,
    DateTime? CreatedAt,
    CreateOutcomeDocumentWithResourcesRequestItem[] Items);

/// <summary>
/// Запрос на создание ресурса отгрузки в документе отгрузки
/// </summary>
/// <param name="ResourceId">Идентификатор ресурса</param>
/// <param name="UnitId">Идентификатор единицы измерения</param>
/// <param name="ResourceQuantity">Количество ресурсов</param>
public record CreateOutcomeDocumentWithResourcesRequestItem(
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity);