using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.ResourceManagement.Contracts.Requests;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.UpdateResource;

public record UpdateResourceCommand(Guid ResourceId, string Title) : ICommand
{
    public static UpdateResourceCommand Create(Guid resourceId, UpdateResourceRequest request) => 
        new (resourceId, request.Title);
}