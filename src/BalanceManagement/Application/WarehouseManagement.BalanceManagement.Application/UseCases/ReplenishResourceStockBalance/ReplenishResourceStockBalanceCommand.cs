using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.ReplenishResourceStockBalance;

public record ReplenishResourceStockBalanceCommand(Guid ResourceId, Guid UnitId, int AddedResourceStock) : ICommand
{
    public static ReplenishResourceStockBalanceCommand Create(Guid resourceId, Guid unitId, ReplenishResourceStockBalanceRequest request) => 
        new(resourceId, unitId, request.AddedResourceStock);
}