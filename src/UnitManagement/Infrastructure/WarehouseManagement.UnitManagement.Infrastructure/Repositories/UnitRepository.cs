using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Domain.Entities;
using WarehouseManagement.UnitManagement.Infrastructure.DbContexts;

namespace WarehouseManagement.UnitManagement.Infrastructure.Repositories;

public class UnitRepository : IRepository<Unit,UnitId>
{
    private readonly WriteDbContext _dbContext;

    public UnitRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnitId> AddAsync(Unit entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Units.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public UnitId Delete(Unit entity)
    {
        _dbContext.Units.Remove(entity);
        return entity.Id;
    }

    public async Task<Result<Unit, Error>> GetByIdAsync(UnitId entityId, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.Units.FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);

        if (resource is null) 
            return Errors.General.NotFound(entityId.Value);
        
        return resource;
    }
}