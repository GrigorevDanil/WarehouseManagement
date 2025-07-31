using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.ResourceManagement.Infrastructure.DbContexts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Infrastructure.Repositories;

public class ResourceRepository : IRepository<Resource, ResourceId>
{
    private readonly WriteDbContext _writeDbContext;

    public ResourceRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<ResourceId> AddAsync(Resource entity, CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Resources.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public ResourceId Delete(Resource entity)
    {
        _writeDbContext.Resources.Remove(entity);
        return entity.Id;
    }

    public async Task<Result<Resource, Error>> GetByIdAsync(ResourceId entityId, CancellationToken cancellationToken = default)
    {
        var resource = await _writeDbContext.Resources.FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);

        if (resource is null) 
            return Errors.General.NotFound(entityId.Value);
        
        return resource;
    }
}