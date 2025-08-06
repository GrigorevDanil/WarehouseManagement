using WarehouseManagement.BalanceManagement.Contracts.Dtos;

namespace WarehouseManagement.BalanceManagement.Application.Interfaces;

public interface IBalanceReadDbContext
{
    IQueryable<BalanceDto> Balances { get; }
}