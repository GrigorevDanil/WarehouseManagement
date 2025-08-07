using MassTransit;
using WarehouseManagement.BalanceManagement.Application.UseCases.CreateBalance;
using WarehouseManagement.BalanceManagement.Application.UseCases.ReplenishResourceStockBalance;
using WarehouseManagement.BalanceManagement.Contracts;
using WarehouseManagement.OutcomeProcessing.Contracts.Messaging;

namespace WarehouseManagement.BalanceManagement.Application.Consumers;

public class RevokeDocumentConsumer : IConsumer<RevokeDocumentEvent>
{
    private readonly IBalanceManagementContract _balanceManagementContract;

    private readonly CreateBalanceHandler _createBalanceHandler;
    
    private readonly ReplenishResourceStockBalanceHandler _replenishResourceStockBalanceHandler;

    public RevokeDocumentConsumer(
        IBalanceManagementContract balanceManagementContract,
        CreateBalanceHandler createBalanceHandler,
        ReplenishResourceStockBalanceHandler replenishResourceStockBalanceHandler)
    {
        _balanceManagementContract = balanceManagementContract;
        _createBalanceHandler = createBalanceHandler;
        _replenishResourceStockBalanceHandler = replenishResourceStockBalanceHandler;
    }

    public async Task Consume(ConsumeContext<RevokeDocumentEvent> context)
    {
        var checkBalanceResourceIdNotExistsResult = 
            await _balanceManagementContract.CheckBalanceByResourceIdAndUnitIdNotExists(
                context.Message.ResourceId,
                context.Message.UnitId);

        if (checkBalanceResourceIdNotExistsResult.IsSuccess)
        {
            var command = new CreateBalanceCommand(
                context.Message.ResourceId,
                context.Message.UnitId,
                context.Message.AddedResourceStock);

            await _createBalanceHandler.Handle(command);
        }
        else
        {
            var command = new ReplenishResourceStockBalanceCommand(
                context.Message.ResourceId,
                context.Message.UnitId,
                context.Message.AddedResourceStock);
            
            await _replenishResourceStockBalanceHandler.Handle(command);
        }
    }
}