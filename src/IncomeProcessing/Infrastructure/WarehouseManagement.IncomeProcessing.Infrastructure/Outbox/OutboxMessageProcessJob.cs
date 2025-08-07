using Microsoft.Extensions.DependencyInjection;
using Quartz;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.IncomeProcessing.Contracts;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class OutboxMessageProcessJob : IJob
{
    private readonly IOutboxMessageProcess _outboxMessageProcess;

    public OutboxMessageProcessJob([FromKeyedServices(Modules.IncomeProcessing)]IOutboxMessageProcess outboxMessageProcess)
    {
        _outboxMessageProcess = outboxMessageProcess;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await _outboxMessageProcess.ExecuteAsync(AssemblyReference.Assembly, context.CancellationToken);
    }
}