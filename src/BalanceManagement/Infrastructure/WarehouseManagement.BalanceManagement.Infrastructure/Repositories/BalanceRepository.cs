using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.BalanceManagement.Application.Interfaces;
using WarehouseManagement.BalanceManagement.Domain.Entities;
using WarehouseManagement.BalanceManagement.Infrastructure.DbContexts;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.BalanceManagement.Infrastructure.Repositories;

public class BalanceRepository : IBalanceRepository
{
    private readonly WriteDbContext _dbContext;

    public BalanceRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<BalanceId> AddAsync(Balance entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Balances.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public BalanceId Delete(Balance entity)
    {
        _dbContext.Balances.Remove(entity);
        return entity.Id;
    }

    public async Task<Result<Balance, Error>> GetByIdAsync(BalanceId entityId, CancellationToken cancellationToken = default)
    {
        var balance = await _dbContext.Balances
            .FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);

        if (balance is null) 
            return Errors.General.NotFound(entityId.Value);
        
        return balance;
    }

    public async Task<Result<Balance, Error>> GetBalanceByResourceIdAndUnitIdAsync(ResourceId resourceId, UnitId unitId,
        CancellationToken cancellationToken = default)
    {
        var balance = await _dbContext.Balances
            .FirstOrDefaultAsync(x => 
                x.ResourceId == resourceId &&
                x.UnitId == unitId,
                cancellationToken);

        if (balance is null) 
            return Errors.General.NotFound(resourceId.Value);
        
        return balance;
    }
    
}