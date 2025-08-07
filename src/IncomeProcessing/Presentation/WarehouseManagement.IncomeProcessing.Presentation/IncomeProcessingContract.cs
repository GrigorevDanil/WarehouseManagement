using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Contracts;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.IncomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Presentation;

public class IncomeProcessingContract : IIncomeProcessingContract
{
    private readonly IIncomeProcessingReadDbContext _dbContext;

    public IncomeProcessingContract(IIncomeProcessingReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnitResult<Error>> CheckIncomeDocumentNumDocumentNotExists(string numDocument)
    {
        var foundedIncomeDocument = await _dbContext.IncomeDocuments
            .FirstOrDefaultAsync(s => s.NumDocument == numDocument);

        if (foundedIncomeDocument is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(IncomeDocument), nameof(numDocument), numDocument);
    }

    public async Task<UnitResult<Error>> CheckIncomeResourceExistsInDocument(Guid incomeDocumentId, Guid resourceId)
    {
        var foundedIncomeResource = await _dbContext.IncomeResources
            .AnyAsync(r => 
                r.IncomeDocumentId == incomeDocumentId && 
                r.ResourceId == resourceId);
        
        if (!foundedIncomeResource)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(IncomeResource), nameof(ResourceId), resourceId.ToString());
    }
    
    public async Task<UnitResult<Error>> CheckResourceIdNotUsedInAnyIncomeResource(ResourceId resourceId)
    {
        var isResourceUsed = await _dbContext.IncomeResources
            .AnyAsync(r => r.ResourceId == resourceId.Value);
    
        if (!isResourceUsed)
            return Result.Success<Error>();

        return Errors.General.AlreadyInUse("Resource", resourceId.Value.ToString());
    }
    
    public async Task<UnitResult<Error>> CheckUnitIdNotUsedInAnyIncomeResource(UnitId unitId)
    {
        var isUnitUsed = await _dbContext.IncomeResources
            .AnyAsync(r => r.UnitId == unitId.Value);
    
        if (!isUnitUsed)
            return Result.Success<Error>();

        return Errors.General.AlreadyInUse("Unit", unitId.Value.ToString());
    }
    
}