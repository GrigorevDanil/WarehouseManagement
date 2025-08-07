using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Contracts;

public interface IOutcomeProcessingContract
{
    Task<UnitResult<Error>> CheckOutcomeDocumentNumDocumentNotExists(string numDocument);
    Task<UnitResult<Error>> CheckResourceIdNotUsedInAnyOutcomeResource(ResourceId resourceId);
    Task<UnitResult<Error>> CheckUnitIdNotUsedInAnyOutcomeResource(UnitId unitId);
    Task<UnitResult<Error>> CheckClientIdNotUsedInAnyIncomeResource(ClientId clientId);
}