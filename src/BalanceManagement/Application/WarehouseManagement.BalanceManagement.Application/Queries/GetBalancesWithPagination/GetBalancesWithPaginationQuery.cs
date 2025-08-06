using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.BalanceManagement.Application.Queries.GetBalancesWithPagination;

public record GetBalancesWithPaginationQuery(
    int Page,
    int PageSize,
    Guid?[] ResourceIds,
    Guid?[] UnitIds,
    string? SortDirection) : IQuery
{
    public static GetBalancesWithPaginationQuery Create(GetBalancesWithPaginationRequest request) => 
        new(request.Page, request.PageSize, request.ResourceIds, request.UnitIds, request.SortDirection);
}