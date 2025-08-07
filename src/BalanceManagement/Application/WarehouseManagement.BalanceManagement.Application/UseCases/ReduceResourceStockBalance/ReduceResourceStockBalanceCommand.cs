using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.ReduceResourceStockBalance;
public record ReduceResourceStockBalanceCommand(Guid ResourceId, Guid UnitId, int SubtractedResourceQuantity) : ICommand
{
    public static ReduceResourceStockBalanceCommand Create(Guid resourceId,Guid unitId, ReduceResourceStockBalanceRequest request) => 
        new(resourceId, unitId, request.SubtractedResourceQuantity);
}