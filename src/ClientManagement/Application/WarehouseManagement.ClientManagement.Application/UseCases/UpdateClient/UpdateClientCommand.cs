using WarehouseManagement.ClientManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.UseCases.UpdateClient;

public record UpdateClientCommand(Guid ClientId, string Title, string Address) : ICommand
{
    public static  UpdateClientCommand Create(Guid clientId, UpdateClientRequest request) => 
        new(clientId, request.Title, request.Address);
}