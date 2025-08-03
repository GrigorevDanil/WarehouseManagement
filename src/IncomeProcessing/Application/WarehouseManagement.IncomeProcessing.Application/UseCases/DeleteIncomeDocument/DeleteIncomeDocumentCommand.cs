using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeDocument;

public record DeleteIncomeDocumentCommand(Guid IncomeDocumentId) : ICommand
{
    public static DeleteIncomeDocumentCommand Create(Guid incomeDocumentId) => 
        new(incomeDocumentId);
}