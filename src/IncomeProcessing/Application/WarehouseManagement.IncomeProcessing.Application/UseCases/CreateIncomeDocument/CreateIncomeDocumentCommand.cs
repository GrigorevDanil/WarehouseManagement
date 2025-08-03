using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.CreateIncomeDocument;

public record CreateIncomeDocumentCommand(string NumDocument, DateTime? CreatedAt) : ICommand
{
    public static CreateIncomeDocumentCommand Create(CreateIncomeDocumentRequest request) => 
        new(request.NumDocument, request.CreatedAt);
}
