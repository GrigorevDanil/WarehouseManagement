using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ClientManagement.Application.Interfaces;
using WarehouseManagement.ClientManagement.Application.Queries.GetClientById;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.ClientManagement.Contracts.Dtos;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ClientManagement.Presentation;

public class ClientManagementContract : IClientManagementContract
{
    private readonly IClientReadDbContext _dbContext;

    private readonly GetClientByIdHandler _getClientByIdHandler;

    public ClientManagementContract(IClientReadDbContext dbContext, GetClientByIdHandler getClientByIdHandler)
    {
        _dbContext = dbContext;
        _getClientByIdHandler = getClientByIdHandler;
    }

    public async Task<UnitResult<Error>> CheckClientTitleNotExists(string title)
    {
        var foundedClient = await _dbContext.Clients
            .FirstOrDefaultAsync(s => s.Title == title);

        if (foundedClient is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(Client), nameof(Title), title);
    }

    public async Task<UnitResult<Error>> CheckClientExistsAndNotArchivedById(Guid clientId)
    {
        var foundedClient = await _dbContext.Clients
            .FirstOrDefaultAsync(s => s.Id == clientId);

        if (foundedClient is null) return Errors.General.NotFound(clientId);
        
        if (foundedClient.IsArchived) return Errors.Archive.ArchiveRecord(nameof(Client), clientId);
        
        return Result.Success<Error>();
    }

    public async Task<Result<ClientDto, ErrorList>> GetClientById(Guid clientId, CancellationToken cancellationToken = default)
    {
        var query = new GetClientByIdQuery(clientId);
        
        return await _getClientByIdHandler.Handle(query, cancellationToken);
    }
}