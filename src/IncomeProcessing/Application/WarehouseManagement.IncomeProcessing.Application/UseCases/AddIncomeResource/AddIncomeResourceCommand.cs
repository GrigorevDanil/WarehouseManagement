using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.AddIncomeResource;

public record AddIncomeResourceCommand(
    Guid IncomeDocumentId,
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity) : ICommand
{
    public static AddIncomeResourceCommand Create(Guid incomeDocumentId,AddIncomeResourceRequest request) => 
        new(incomeDocumentId, request.ResourceId, request.UnitId, request.ResourceQuantity);
}