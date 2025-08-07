using CSharpFunctionalExtensions;
using WarehouseManagement.ClientManagement.Contracts.Dtos;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.ClientManagement.Contracts;

public interface IClientManagementContract
{
    Task<UnitResult<Error>> CheckClientTitleNotExists(string title);
    
    Task<UnitResult<Error>> CheckClientExistsAndNotArchivedById(Guid clientId);
    
    Task<Result<ClientDto, ErrorList>> GetClientById(
        Guid clientId, CancellationToken cancellationToken = default);
}