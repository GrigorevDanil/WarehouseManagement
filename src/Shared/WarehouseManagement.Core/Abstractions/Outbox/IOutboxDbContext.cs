using Microsoft.EntityFrameworkCore;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.Core.Abstractions.Outbox;

public interface IOutboxDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}