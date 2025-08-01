using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.Queries.GetClientById;

public record GetClientByIdQuery(Guid ClientId) : IQuery
{
    public static GetClientByIdQuery Create(Guid clientId) => 
        new(clientId);
}