using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.Core.Models;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;

namespace WarehouseManagement.ResourceManagement.Application.Queries.GetResourcesWithPagination;

public class GetResourcesWithPaginationHandler : IQueryHandler<PagedList<ResourceDto>, GetResourcesWithPaginationQuery>
{
    private readonly IResourceReadDbContext _dbContext;

    public GetResourcesWithPaginationHandler(IResourceReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedList<ResourceDto>> Handle(GetResourcesWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var resourceQuery =  _dbContext.Resources;

        resourceQuery = resourceQuery
            .WhereIf(!string.IsNullOrWhiteSpace(query.Title), res => res.Title.Contains(query.Title!))
            .WhereIf(query.IsArchive.HasValue, res => res.IsArchived == query.IsArchive);
        
        resourceQuery = query.SortDirection?.ToLower() == "desc"
            ? resourceQuery.OrderByDescending(res => res.Title)
            : resourceQuery.OrderBy(res => res.Title);

        return resourceQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
    
}