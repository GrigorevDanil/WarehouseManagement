namespace WarehouseManagement.OutcomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на обновление документа отгрузки
/// </summary>
/// <param name="NumDocument">Номер документа</param>
/// <param name="ClientId">Идентификатор клиента</param>
/// <param name="CreatedAt">Создан в</param>
public record UpdateOutcomeDocumentRequest(string NumDocument, Guid ClientId, DateTime? CreatedAt);