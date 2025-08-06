using CSharpFunctionalExtensions;
using WarehouseManagement.BalanceManagement.Domain.Entities;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.BalanceManagement.Application.Interfaces;

public interface IBalanceRepository : IRepository<Balance, BalanceId>
{
    Task<Result<Balance, Error>> GetBalanceByResourceIdAndUnitIdAsync(ResourceId resourceId, UnitId unitId, CancellationToken cancellationToken = default);
}