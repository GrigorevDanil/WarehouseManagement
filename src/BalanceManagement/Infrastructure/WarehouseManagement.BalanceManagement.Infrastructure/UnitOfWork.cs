using WarehouseManagement.BalanceManagement.Infrastructure.DbContexts;
using WarehouseManagement.Core.Abstractions;

namespace WarehouseManagement.BalanceManagement.Infrastructure;

public class UnitOfWork: IUnitOfWork
{
    private readonly WriteDbContext _dbContext;

    public UnitOfWork(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);
}