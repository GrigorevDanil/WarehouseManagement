using WarehouseManagement.ResourceManagement.Contracts.Dtos;

namespace WarehouseManagement.ResourceManagement.Application.Interfaces;

public interface IResourceReadDbContext
{
    IQueryable<ResourceDto> Resources { get; }
}