using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.ClientManagement.Contracts;

public interface IClientManagementContract
{
    Task<UnitResult<Error>> CheckClientTitleNotExists(string title);
}