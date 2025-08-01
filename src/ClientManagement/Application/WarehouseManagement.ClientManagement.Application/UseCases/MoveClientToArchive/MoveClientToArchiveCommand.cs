using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.UseCases.MoveClientToArchive;

public record MoveClientToArchiveCommand(Guid ClientId) : ICommand
{
    public static MoveClientToArchiveCommand Create(Guid clientId) => 
        new(clientId);
}