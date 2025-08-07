using MassTransit;
using WarehouseManagement.BalanceManagement.Application.UseCases.ReduceResourceStockBalance;
using WarehouseManagement.OutcomeProcessing.Contracts.Messaging;

namespace WarehouseManagement.BalanceManagement.Application.Consumers;

public class SignDocumentConsumer : IConsumer<SignDocumentEvent>
{
    private readonly ReduceResourceStockBalanceHandler _reduceResourceStockBalanceHandler;

    public SignDocumentConsumer(ReduceResourceStockBalanceHandler reduceResourceStockBalanceHandler)
    {
        _reduceResourceStockBalanceHandler = reduceResourceStockBalanceHandler;
    }

    public async Task Consume(ConsumeContext<SignDocumentEvent> context)
    {
        var command = new ReduceResourceStockBalanceCommand(
            context.Message.ResourceId,
            context.Message.UnitId,
            context.Message.SubtractedResourceQuantity);
        
        await _reduceResourceStockBalanceHandler.Handle(command);
    }
}