using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.IncomeProcessing.Infrastructure.DbContexts;

namespace WarehouseManagement.IncomeProcessing.Infrastructure;

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