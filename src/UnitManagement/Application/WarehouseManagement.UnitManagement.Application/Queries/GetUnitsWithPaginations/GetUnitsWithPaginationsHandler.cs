using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.Core.Models;
using WarehouseManagement.UnitManagement.Application.Interfaces;
using WarehouseManagement.UnitManagement.Contracts.Dtos;

namespace WarehouseManagement.UnitManagement.Application.Queries.GetUnitsWithPaginations;

public class GetUnitsWithPaginationsHandler : IQueryHandler<PagedList<UnitDto>, GetUnitsWithPaginationsQuery>
{
    private readonly IUnitReadDbContext _dbContext;

    public GetUnitsWithPaginationsHandler(IUnitReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedList<UnitDto>> Handle(GetUnitsWithPaginationsQuery query, CancellationToken cancellationToken = default)
    {
        var unitQuery =  _dbContext.Units;

        unitQuery = unitQuery
            .WhereIf(!string.IsNullOrWhiteSpace(query.Title), res => res.Title.Contains(query.Title!))
            .WhereIf(query.IsArchive.HasValue, res => res.IsArchived == query.IsArchive);
        
        unitQuery = query.SortDirection?.ToLower() == "desc"
            ? unitQuery.OrderByDescending(res => res.Title)
            : unitQuery.OrderBy(res => res.Title);

        return unitQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}