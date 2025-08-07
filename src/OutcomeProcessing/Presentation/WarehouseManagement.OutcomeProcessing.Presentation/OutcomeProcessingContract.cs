using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.OutcomeProcessing.Application.Interfaces;
using WarehouseManagement.OutcomeProcessing.Contracts;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.SharedKernel;

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
}