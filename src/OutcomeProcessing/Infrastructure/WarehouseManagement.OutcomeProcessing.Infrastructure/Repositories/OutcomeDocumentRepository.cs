using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Infrastructure.DbContexts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.Repositories;

public class OutcomeDocumentRepository : IRepository<OutcomeDocument,OutcomeDocumentId>
{
    private readonly WriteDbContext _dbContext;

    public OutcomeDocumentRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OutcomeDocumentId> AddAsync(OutcomeDocument entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.OutcomeDocuments.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public OutcomeDocumentId Delete(OutcomeDocument entity)
    {
        _dbContext.OutcomeDocuments.Remove(entity);
        return entity.Id;
    }

    public async Task<Result<OutcomeDocument, Error>> GetByIdAsync(OutcomeDocumentId entityId, CancellationToken cancellationToken = default)
    {
        var outcomeDocument = await _dbContext.OutcomeDocuments
            .Include(x => x.Resources)
            .FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);

        if (outcomeDocument is null) 
            return Errors.General.NotFound(entityId.Value);
        
        return outcomeDocument;
    }
}