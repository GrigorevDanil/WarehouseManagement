using CSharpFunctionalExtensions;
using WarehouseManagement.BalanceManagement.Contracts.Dtos;
using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.BalanceManagement.Contracts;

public interface IBalanceManagementContract
{
    Task<Result<BalanceDto, Error>> GetBalanceByResourceIdAndUnitId(Guid resourceId, Guid unitId);
    Task<UnitResult<Error>> CheckBalanceByResourceIdAndUnitIdNotExists(Guid resourceId, Guid unitId);
    
}