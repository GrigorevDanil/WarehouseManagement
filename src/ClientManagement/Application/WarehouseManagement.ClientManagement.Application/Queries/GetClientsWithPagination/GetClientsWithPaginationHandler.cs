using WarehouseManagement.ClientManagement.Application.Interfaces;
using WarehouseManagement.ClientManagement.Contracts.Dtos;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.Core.Models;

namespace WarehouseManagement.ClientManagement.Application.Queries.GetClientsWithPagination;

public class GetClientsWithPaginationHandler : IQueryHandler<PagedList<ClientDto>, GetClientsWithPaginationQuery>
{
    private readonly IClientReadDbContext _dbContext;

    public GetClientsWithPaginationHandler(IClientReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedList<ClientDto>> Handle(GetClientsWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var clientQuery = _dbContext.Clients;
        
        clientQuery = clientQuery
            .WhereIf(!string.IsNullOrWhiteSpace(query.Title), res => res.Title.Contains(query.Title!))
            .WhereIf(query.IsArchive.HasValue, res => res.IsArchived == query.IsArchive);
        
        clientQuery = query.SortDirection?.ToLower() == "desc"
            ? clientQuery.OrderByDescending(res => res.Title)
            : clientQuery.OrderBy(res => res.Title);

        return clientQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}