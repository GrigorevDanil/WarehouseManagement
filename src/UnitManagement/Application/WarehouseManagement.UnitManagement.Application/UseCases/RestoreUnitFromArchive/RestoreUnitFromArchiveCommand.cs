using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.UnitManagement.Application.UseCases.RestoreUnitFromArchive;

public record RestoreUnitFromArchiveCommand(Guid UnitId) : ICommand
{
    public static RestoreUnitFromArchiveCommand Create(Guid unitId) => 
        new(unitId);
}