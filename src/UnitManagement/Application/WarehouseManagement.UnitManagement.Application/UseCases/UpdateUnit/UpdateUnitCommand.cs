using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.UnitManagement.Contracts.Requests;

namespace WarehouseManagement.UnitManagement.Application.UseCases.UpdateUnit;

public record UpdateUnitCommand(Guid UnitId, string Title) : ICommand 
{
    public static UpdateUnitCommand Create(Guid unitId, UpdateUnitRequest request) => 
        new (unitId, request.Title);
}