using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ResourceManagement.Application.Queries.GetResourcesById;

public record GetResourcesByIdQuery(Guid ResourceId) : IQuery
{
    public static GetResourcesByIdQuery Create(Guid resourceId) => 
        new(resourceId);
}