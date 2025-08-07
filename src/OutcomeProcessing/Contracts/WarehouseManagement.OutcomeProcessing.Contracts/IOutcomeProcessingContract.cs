using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.OutcomeProcessing.Contracts;

public interface IOutcomeProcessingContract
{
    Task<UnitResult<Error>> CheckOutcomeDocumentNumDocumentNotExists(string numDocument);
}