using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.UnitManagement.Application.Interfaces;
using WarehouseManagement.UnitManagement.Application.Queries.GetUnitById;
using WarehouseManagement.UnitManagement.Contracts;
using WarehouseManagement.UnitManagement.Contracts.Dtos;
using WarehouseManagement.UnitManagement.Domain.Entities;

namespace WarehouseManagement.UnitManagement.Presentation;

public class UnitManagementContract : IUnitManagementContract
{
    private readonly IUnitReadDbContext _dbContext;

    private readonly GetUnitByIdHandler _getUnitByIdHandler;

    public UnitManagementContract(
        IUnitReadDbContext dbContext,
        GetUnitByIdHandler getUnitByIdHandler)
    {
        _dbContext = dbContext;
        _getUnitByIdHandler = getUnitByIdHandler;
    }

    public async Task<UnitResult<Error>> CheckUnitTitleNotExists(string title)
    {
        var foundedUnit = await _dbContext.Units
            .FirstOrDefaultAsync(s => s.Title == title);

        if (foundedUnit is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(Unit), nameof(Title), title);
    }

    public async Task<UnitResult<Error>> CheckUnitExistsAndNotArchivedById(Guid unitId)
    {
        var foundedUnit = await _dbContext.Units
            .FirstOrDefaultAsync(s => s.Id == unitId);

        if (foundedUnit is null) return Errors.General.NotFound(unitId);
        
        if (foundedUnit.IsArchived) return Errors.Archive.ArchiveRecord(nameof(Unit), unitId);
        
        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> CheckUnitExistsById(Guid unitId)
    {
        var foundedUnit = await _dbContext.Units
            .FirstOrDefaultAsync(s => s.Id == unitId);

        if (foundedUnit is null) return Errors.General.NotFound(unitId);
        
        
        return Result.Success<Error>();
    }

    public async Task<Result<UnitDto, ErrorList>> GetUnitById(Guid unitId, CancellationToken cancellationToken = default)
    {
        var query = GetUnitByIdQuery.Create(unitId);
        
        return await _getUnitByIdHandler.Handle(query, cancellationToken);
    }
}