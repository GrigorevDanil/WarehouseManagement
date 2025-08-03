using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeDocument;

public record UpdateIncomeDocumentCommand(Guid IncomeDocumentId, string NumDocument, DateTime? CreatedAt) : ICommand
{
    public static UpdateIncomeDocumentCommand Create(Guid incomeDocumentId, UpdateIncomeDocumentRequest request) => 
        new(incomeDocumentId, request.NumDocument, request.CreatedAt);
}