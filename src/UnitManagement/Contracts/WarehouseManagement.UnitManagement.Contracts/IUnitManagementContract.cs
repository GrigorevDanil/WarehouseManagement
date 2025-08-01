using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.UnitManagement.Contracts;

public interface IUnitManagementContract
{
    Task<UnitResult<Error>> CheckUnitTitleNotExists(string title);
}