using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.OutcomeProcessing.Application.Interfaces;
using WarehouseManagement.OutcomeProcessing.Contracts;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Presentation;

public class OutcomeProcessingContract : IOutcomeProcessingContract
{
    private readonly IOutcomeProcessingReadDbContext _dbContext;

    public OutcomeProcessingContract(IOutcomeProcessingReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<UnitResult<Error>> CheckOutcomeDocumentNumDocumentNotExists(string numDocument)
    {
        var foundedOutcomeDocument = await _dbContext.OutcomeDocuments
            .FirstOrDefaultAsync(s => s.NumDocument == numDocument);

        if (foundedOutcomeDocument is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(OutcomeDocument), nameof(numDocument), numDocument);
    }
    
    public async Task<UnitResult<Error>> CheckResourceIdNotUsedInAnyOutcomeResource(ResourceId resourceId)
    {
        var isResourceUsed = await _dbContext.OutcomeResources
            .AnyAsync(r => r.ResourceId == resourceId.Value);
    
        if (!isResourceUsed)
            return Result.Success<Error>();

        return Errors.General.AlreadyInUse("Resource", resourceId.Value.ToString());
    }
    
    public async Task<UnitResult<Error>> CheckUnitIdNotUsedInAnyOutcomeResource(UnitId unitId)
    {
        var isUnitUsed = await _dbContext.OutcomeResources
            .AnyAsync(r => r.UnitId == unitId.Value);
    
        if (!isUnitUsed)
            return Result.Success<Error>();

        return Errors.General.AlreadyInUse("Unit", unitId.Value.ToString());
    }
    
    public async Task<UnitResult<Error>> CheckClientIdNotUsedInAnyIncomeResource(ClientId clientId)
    {
        var isClientUsed = await _dbContext.OutcomeDocuments
            .AnyAsync(r => r.ClientId == clientId.Value);
    
        if (!isClientUsed)
            return Result.Success<Error>();

        return Errors.General.AlreadyInUse("Client", clientId.Value.ToString());
    }
}