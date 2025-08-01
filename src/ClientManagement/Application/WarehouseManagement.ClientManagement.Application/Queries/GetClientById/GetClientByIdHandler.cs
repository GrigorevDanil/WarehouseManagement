using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ClientManagement.Application.Interfaces;
using WarehouseManagement.ClientManagement.Contracts.Dtos;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.ClientManagement.Application.Queries.GetClientById;

public class GetClientByIdHandler : IQueryHandlerWithResult<ClientDto,  GetClientByIdQuery>
{
    private readonly IClientReadDbContext _dbContext;

    public GetClientByIdHandler(IClientReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ClientDto, ErrorList>> Handle(GetClientByIdQuery query, CancellationToken cancellationToken = default)
    {
        var  client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id == query.ClientId, cancellationToken);
        
        if (client == null)
            return Errors.General.NotFound(query.ClientId).ToErrorList();

        return client;
    }
}