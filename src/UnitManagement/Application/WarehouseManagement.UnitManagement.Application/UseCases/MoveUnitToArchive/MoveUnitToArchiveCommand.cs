using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.UnitManagement.Application.UseCases.MoveUnitToArchive;

public record MoveUnitToArchiveCommand(Guid UnitId) : ICommand
{
    public static MoveUnitToArchiveCommand Create(Guid unitId) =>
        new(unitId);
}