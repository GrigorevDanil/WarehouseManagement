using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.ClientManagement.Infrastructure.DbContexts;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ClientManagement.Infrastructure.Repositories;

public class ClientRepository : IRepository<Client, ClientId>
{
    private readonly WriteDbContext _dbContext;

    public ClientRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ClientId> AddAsync(Client entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Clients.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public ClientId Delete(Client entity)
    {
        _dbContext.Clients.Remove(entity);
        return entity.Id;
    }

    public async Task<Result<Client, Error>> GetByIdAsync(ClientId entityId, CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);

        if (client is null) 
            return Errors.General.NotFound(entityId.Value);
        
        return client;
    }
}