namespace WarehouseManagement.IncomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на создание документа поступления
/// </summary>
/// <param name="NumDocument">Номер документа поступления</param>
/// <param name="CreatedAt">Дата создания документа поступления</param>
public record CreateIncomeDocumentRequest(string NumDocument, DateTime? CreatedAt);