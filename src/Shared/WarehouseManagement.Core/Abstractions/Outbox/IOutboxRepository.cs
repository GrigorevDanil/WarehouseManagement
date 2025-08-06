namespace WarehouseManagement.Core.Abstractions.Outbox;

public interface IOutboxRepository
{
    Task AddAsync<T>(T message, CancellationToken cancellationToken = default);
}