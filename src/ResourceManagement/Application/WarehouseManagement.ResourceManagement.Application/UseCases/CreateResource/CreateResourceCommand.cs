using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.ResourceManagement.Contracts.Requests;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.CreateResource;

public record CreateResourceCommand(string Title) : ICommand
{
    public static CreateResourceCommand Create(CreateResourceRequest request) => 
        new (request.Title);
}