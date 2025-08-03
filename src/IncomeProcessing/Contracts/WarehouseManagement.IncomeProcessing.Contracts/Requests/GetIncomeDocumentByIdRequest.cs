namespace WarehouseManagement.IncomeProcessing.Contracts.Requests;

/// <summary>
/// Запрос на получение документа поступления по идентификатору
/// </summary>
/// <param name="ResourceIds">Идентификаторы ресурсов которые есть в документе</param>
/// <param name="UnitIds">Идентификаторы единиц измерения которые есть в документе</param>
public record GetIncomeDocumentByIdRequest(
    Guid?[] ResourceIds,
    Guid?[] UnitIds);