using MassTransit;
using WarehouseManagement.BalanceManagement.Application.UseCases.ReduceResourceStockBalance;
using WarehouseManagement.BalanceManagement.Application.UseCases.ReplenishResourceStockBalance;
using WarehouseManagement.IncomeProcessing.Contracts.Messaging;

namespace WarehouseManagement.BalanceManagement.Application.Consumers;

public class UpdateIncomeResourceEventConsumer : IConsumer<UpdateIncomeResourceEvent>
{
    private readonly ReplenishResourceStockBalanceHandler _replenishResourceStockBalanceHandler;
    
    private readonly ReduceResourceStockBalanceHandler _reduceResourceStockBalanceHandler;

    public UpdateIncomeResourceEventConsumer(
        ReplenishResourceStockBalanceHandler replenishResourceStockBalanceHandler,
        ReduceResourceStockBalanceHandler reduceResourceStockBalanceHandler)
    {
        _replenishResourceStockBalanceHandler = replenishResourceStockBalanceHandler;
        _reduceResourceStockBalanceHandler = reduceResourceStockBalanceHandler;
    }

    public async Task Consume(ConsumeContext<UpdateIncomeResourceEvent> context)
    {
        if (context.Message.UpdatedResourceQuantity > 0)
        {
            var command = new ReplenishResourceStockBalanceCommand(
                context.Message.ResourceId,
                context.Message.UnitId,
                context.Message.UpdatedResourceQuantity);
            
            await _replenishResourceStockBalanceHandler.Handle(command);
        }
        else
        {
            var quantity = context.Message.UpdatedResourceQuantity * -1;
            
            var command = new ReduceResourceStockBalanceCommand(
                context.Message.ResourceId,
                context.Message.UnitId,
                quantity);
            
            await _reduceResourceStockBalanceHandler.Handle(command);
        }
    }
}