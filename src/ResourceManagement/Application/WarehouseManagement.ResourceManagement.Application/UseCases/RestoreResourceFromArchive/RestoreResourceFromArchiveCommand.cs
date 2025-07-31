using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.RestoreResourceFromArchive;

public record RestoreResourceFromArchiveCommand(Guid ResourceId) : ICommand
{
    public static RestoreResourceFromArchiveCommand Create(Guid resourceId) =>  
        new(resourceId);
}