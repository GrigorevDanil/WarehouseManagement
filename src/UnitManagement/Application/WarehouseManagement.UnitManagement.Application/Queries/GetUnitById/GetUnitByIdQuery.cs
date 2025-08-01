using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.UnitManagement.Application.Queries.GetUnitById;

public record GetUnitByIdQuery(Guid UnitId) : IQuery
{
    public static GetUnitByIdQuery Create(Guid unitId) => 
        new(unitId);
}