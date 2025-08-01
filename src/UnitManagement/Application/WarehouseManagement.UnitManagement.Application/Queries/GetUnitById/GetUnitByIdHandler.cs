using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Application.Interfaces;
using WarehouseManagement.UnitManagement.Contracts.Dtos;

namespace WarehouseManagement.UnitManagement.Application.Queries.GetUnitById;

public class GetUnitByIdHandler : IQueryHandlerWithResult<UnitDto, GetUnitByIdQuery>
{
    private readonly IUnitReadDbContext _dbContext;

    public GetUnitByIdHandler(IUnitReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UnitDto, ErrorList>> Handle(GetUnitByIdQuery query, CancellationToken cancellationToken = default)
    {
        var unit = await _dbContext.Units.FirstOrDefaultAsync(x => x.Id == query.UnitId, cancellationToken);
        
        if (unit == null) 
            return Errors.General.NotFound(query.UnitId).ToErrorList();

        return unit;
    }
}