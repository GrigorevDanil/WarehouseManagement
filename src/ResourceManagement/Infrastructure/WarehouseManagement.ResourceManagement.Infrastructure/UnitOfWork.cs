using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.ResourceManagement.Infrastructure.DbContexts;

namespace WarehouseManagement.ResourceManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _dbContext;

    public UnitOfWork(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);
}