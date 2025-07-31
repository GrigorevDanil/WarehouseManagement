using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.ResourceManagement.Contracts.Requests;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.MoveResourceToArchive;

public record MoveResourceToArchiveCommand(Guid ResourceId) : ICommand
{
    public static MoveResourceToArchiveCommand Create(Guid resourceId) => 
        new(resourceId);
}