using CSharpFunctionalExtensions;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.ResourceManagement.Contracts;

public interface IResourceManagementContract
{
    Task<UnitResult<Error>> CheckResourceTitleNotExists(string title);
    Task<UnitResult<Error>> CheckResourceExistsAndNotArchivedById(Guid resourceId);
    
    Task<Result<ResourceDto, ErrorList>> GetResourceById(
        Guid resourceId, CancellationToken cancellationToken = default);
}