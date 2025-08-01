using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.UnitManagement.Application.Interfaces;
using WarehouseManagement.UnitManagement.Contracts;
using WarehouseManagement.UnitManagement.Domain.Entities;

namespace WarehouseManagement.UnitManagement.Presentation;

public class UnitManagementContract : IUnitManagementContract
{
    private readonly IUnitReadDbContext _dbContext;

    public UnitManagementContract(IUnitReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnitResult<Error>> CheckUnitTitleNotExists(string title)
    {
        var foundedResources = await _dbContext.Units
            .FirstOrDefaultAsync(s => s.Title == title);

        if (foundedResources is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(Unit), nameof(Title), title);
    }
}