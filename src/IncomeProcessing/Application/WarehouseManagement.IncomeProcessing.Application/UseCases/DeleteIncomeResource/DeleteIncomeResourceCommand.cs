using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeResource;

public record DeleteIncomeResourceCommand(Guid IncomeDocumentId, Guid IncomeResourceId) : ICommand
{
    public static DeleteIncomeResourceCommand Create(Guid incomeDocumentId, Guid incomeResourceId) =>  
        new(incomeDocumentId, incomeResourceId);
}