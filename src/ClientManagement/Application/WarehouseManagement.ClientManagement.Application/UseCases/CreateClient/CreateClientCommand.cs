using WarehouseManagement.ClientManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.UseCases.CreateClient;

public record CreateClientCommand(string Title, string Address) : ICommand
{
    public static CreateClientCommand Create(CreateClientRequest request) =>  
        new(request.Title, request.Address);
}