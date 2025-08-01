using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.UseCases.DeleteClient;

public record DeleteClientCommand(Guid ClientId) : ICommand
{
    public static DeleteClientCommand Create(Guid clientId) => 
        new(clientId);
}