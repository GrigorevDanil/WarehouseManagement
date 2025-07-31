using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.ResourceManagement.Contracts;

public interface IResourceManagementContract
{
    Task<UnitResult<Error>> CheckResourceTitleNotExists(string title);
}