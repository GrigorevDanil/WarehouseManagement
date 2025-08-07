using MassTransit;
using WarehouseManagement.BalanceManagement.Application.UseCases.ReduceResourceStockBalance;
using WarehouseManagement.IncomeProcessing.Contracts.Messaging;

namespace WarehouseManagement.BalanceManagement.Application.Consumers;

public class DeleteIncomeResourceEventConsumer: IConsumer<DeleteIncomeResourceEvent>
{
    private readonly ReduceResourceStockBalanceHandler _reduceResourceStockBalanceHandler;

    public DeleteIncomeResourceEventConsumer(ReduceResourceStockBalanceHandler reduceResourceStockBalanceHandler)
    {
        _reduceResourceStockBalanceHandler = reduceResourceStockBalanceHandler;
    }

    public async Task Consume(ConsumeContext<DeleteIncomeResourceEvent> context)
    {
        var command = new ReduceResourceStockBalanceCommand(
            context.Message.ResourceId,
            context.Message.UnitId,
            context.Message.SubtractedResourceQuantity);
        
        await _reduceResourceStockBalanceHandler.Handle(command);
    }
}