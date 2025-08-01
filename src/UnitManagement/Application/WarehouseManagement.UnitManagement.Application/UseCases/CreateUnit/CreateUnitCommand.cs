using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.UnitManagement.Contracts.Requests;

namespace WarehouseManagement.UnitManagement.Application.UseCases.CreateUnit;

public record CreateUnitCommand(string Title) : ICommand
{
    public static CreateUnitCommand Create(CreateUnitRequest request) => 
        new (request.Title);
}