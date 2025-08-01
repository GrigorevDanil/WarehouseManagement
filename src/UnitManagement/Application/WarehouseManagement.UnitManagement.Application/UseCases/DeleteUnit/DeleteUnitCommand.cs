using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.UnitManagement.Application.UseCases.DeleteUnit;

public record DeleteUnitCommand(Guid UnitId) : ICommand
{
    public static DeleteUnitCommand Create(Guid unitId) =>  
        new(unitId);
}