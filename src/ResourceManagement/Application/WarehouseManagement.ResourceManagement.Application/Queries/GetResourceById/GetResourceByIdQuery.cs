using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ResourceManagement.Application.Queries.GetResourceById;

public record GetResourceByIdQuery(Guid ResourceId) : IQuery
{
    public static GetResourceByIdQuery Create(Guid resourceId) => 
        new(resourceId);
}