using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.IncomeProcessing.Infrastructure.DbContexts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Repositories;

public class IncomeDocumentRepository: IRepository<IncomeDocument, IncomeDocumentId>
{
    private readonly WriteDbContext _dbContext;

    public IncomeDocumentRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IncomeDocumentId> AddAsync(IncomeDocument entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.IncomeDocuments.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public IncomeDocumentId Delete(IncomeDocument entity)
    {
        _dbContext.IncomeDocuments.Remove(entity);
        return entity.Id;
    }

    public async Task<Result<IncomeDocument, Error>> GetByIdAsync(IncomeDocumentId entityId, CancellationToken cancellationToken = default)
    {
        var incomeDocument = await _dbContext.IncomeDocuments
            .Include(x => x.Resources)
            .FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);

        if (incomeDocument is null) 
            return Errors.General.NotFound(entityId.Value);
        
        return incomeDocument;
    }
}