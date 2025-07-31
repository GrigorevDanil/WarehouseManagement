using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.ResourceManagement.Contracts.Requests;

namespace WarehouseManagement.ResourceManagement.Application.Queries.GetResourcesWithPagination;

public record GetResourcesWithPaginationQuery(
    int Page,
    int PageSize,
    string? Title,
    string? SortDirection,
    bool? IsArchive) : IQuery
{
    public static GetResourcesWithPaginationQuery Create(GetResourcesWithPaginationRequest request) => 
        new(request.Page, request.PageSize, request.Title, request.SortDirection, request.IsArchive);
}