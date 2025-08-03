using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.IncomeProcessing.Contracts;

public interface IIncomeProcessingContract
{
    Task<UnitResult<Error>> CheckIncomeDocumentNumDocumentNotExists(string numDocument);
    Task<UnitResult<Error>> CheckIncomeResourceExistsInDocument(Guid incomeDocumentId ,Guid resourceId);
}