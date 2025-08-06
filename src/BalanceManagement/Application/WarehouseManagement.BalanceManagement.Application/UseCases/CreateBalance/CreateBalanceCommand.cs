using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.CreateBalance;

public record CreateBalanceCommand(Guid ResourceId, Guid UnitId, int ResourceStock) : ICommand
{
    public static CreateBalanceCommand Create(CreateBalanceRequest request) => 
        new(request.ResourceId, request.UnitId, request.ResourceStock);
}