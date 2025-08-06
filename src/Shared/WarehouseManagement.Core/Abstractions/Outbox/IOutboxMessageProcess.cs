using System.Reflection;

namespace WarehouseManagement.Core.Abstractions.Outbox;

public interface IOutboxMessageProcess
{
    Task ExecuteAsync(Assembly messagesAssembly, CancellationToken cancellationToken = default);
}