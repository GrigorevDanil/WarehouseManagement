using System.Text.Json;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.IncomeProcessing.Infrastructure.DbContexts;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly WriteDbContext _dbContext;

    public OutboxRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message)
        };
        
        await _dbContext.AddAsync(outboxMessage, cancellationToken);
    }
}