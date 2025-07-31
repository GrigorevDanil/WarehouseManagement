using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.DeleteResource;

public record DeleteResourceCommand(Guid ResourceId) : ICommand
{
    public static DeleteResourceCommand Create(Guid resourceId) =>  
        new(resourceId);
}