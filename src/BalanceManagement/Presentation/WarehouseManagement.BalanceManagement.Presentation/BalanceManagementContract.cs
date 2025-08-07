using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.BalanceManagement.Application.Interfaces;
using WarehouseManagement.BalanceManagement.Application.UseCases.CreateBalance;
using WarehouseManagement.BalanceManagement.Contracts;
using WarehouseManagement.BalanceManagement.Contracts.Dtos;
using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.BalanceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.BalanceManagement.Presentation;

public class BalanceManagementContract : IBalanceManagementContract
{
    private readonly IBalanceReadDbContext _dbContext;

    public BalanceManagementContract(IBalanceReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BalanceDto, Error>> GetBalanceByResourceIdAndUnitId(Guid resourceId, Guid unitId)
    {
        var balance = await _dbContext.Balances.FirstOrDefaultAsync(
            x => 
                x.ResourceId == resourceId &&
                x.UnitId == unitId);

        if (balance is null)
            return Errors.General.NotFound();
        
        return balance;
    }

    public async Task<UnitResult<Error>> CheckBalanceByResourceIdAndUnitIdNotExists(Guid resourceId, Guid unitId)
    {
        var balance = await _dbContext.Balances.FirstOrDefaultAsync(
            x => 
                x.ResourceId == resourceId &&
                x.UnitId == unitId);

        if (balance is null)
            return UnitResult.Success<Error>();
        
        return Errors.General.AlreadyExists(nameof(Balance), nameof(BalanceId),  balance.Id.ToString());
    }
}