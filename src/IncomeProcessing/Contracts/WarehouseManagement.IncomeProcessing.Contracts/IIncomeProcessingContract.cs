using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Contracts;

public interface IIncomeProcessingContract
{
    Task<UnitResult<Error>> CheckIncomeDocumentNumDocumentNotExists(string numDocument);
    Task<UnitResult<Error>> CheckIncomeResourceExistsInDocument(Guid incomeDocumentId ,Guid resourceId);
    Task<UnitResult<Error>> CheckResourceIdNotUsedInAnyIncomeResource(ResourceId resourceId);
    Task<UnitResult<Error>> CheckUnitIdNotUsedInAnyIncomeResource(UnitId unitId);
}