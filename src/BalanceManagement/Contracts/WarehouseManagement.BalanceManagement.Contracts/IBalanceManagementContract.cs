using CSharpFunctionalExtensions;
using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.BalanceManagement.Contracts;

public interface IBalanceManagementContract
{
    Task<UnitResult<Error>> CheckBalanceByResourceIdAndUnitIdNotExists(Guid resourceId, Guid unitId);
    
}