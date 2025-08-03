using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Contracts.Dtos;

namespace WarehouseManagement.UnitManagement.Contracts;

public interface IUnitManagementContract
{
    Task<UnitResult<Error>> CheckUnitTitleNotExists(string title);
    Task<UnitResult<Error>> CheckUnitExistsAndNotArchivedById(Guid unitId);
    Task<Result<UnitDto, ErrorList>> GetUnitById(
        Guid unitId, CancellationToken cancellationToken = default);
}