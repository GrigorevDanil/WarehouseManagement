using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.UseCases.RestoreClientFromArchive;

public record RestoreClientFromArchiveCommand(Guid ClientId) : ICommand
{
    public static RestoreClientFromArchiveCommand Create(Guid clientId) =>
        new(clientId);
}