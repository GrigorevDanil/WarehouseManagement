using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeResource;

public record UpdateIncomeResourceCommand(
    Guid IncomeDocumentId,
    Guid IncomeResourceId,
    Guid ResourceId,
    Guid UnitId,
    int ResourceQuantity) : ICommand
{
    public static UpdateIncomeResourceCommand Create(Guid incomeDocumentId, Guid incomeResourceId, UpdateIncomeResourceRequest request) => 
        new(incomeDocumentId, incomeResourceId,request.ResourceId,  request.UnitId, request.ResourceQuantity);
}