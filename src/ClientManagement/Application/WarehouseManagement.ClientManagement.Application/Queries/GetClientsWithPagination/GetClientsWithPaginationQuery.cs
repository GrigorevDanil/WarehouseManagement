using WarehouseManagement.ClientManagement.Contracts.Requests;
using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.ClientManagement.Application.Queries.GetClientsWithPagination;

public record GetClientsWithPaginationQuery(
    int Page,
    int PageSize,
    string? Title,
    string? SortDirection,
    bool? IsArchive) : IQuery
{
    public static GetClientsWithPaginationQuery Create(GetClientsWithPaginationRequest request) => 
        new(request.Page, request.PageSize, request.Title, request.SortDirection, request.IsArchive);
}