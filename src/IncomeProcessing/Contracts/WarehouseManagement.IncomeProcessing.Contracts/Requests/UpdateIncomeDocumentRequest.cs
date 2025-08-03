namespace WarehouseManagement.IncomeProcessing.Contracts.Requests;

public record UpdateIncomeDocumentRequest(string NumDocument, DateTime? CreatedAt);