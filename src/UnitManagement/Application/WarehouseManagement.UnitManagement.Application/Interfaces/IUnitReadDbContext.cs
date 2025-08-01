using WarehouseManagement.UnitManagement.Contracts.Dtos;

namespace WarehouseManagement.UnitManagement.Application.Interfaces;

public interface IUnitReadDbContext
{
    IQueryable<UnitDto> Units { get; }
}