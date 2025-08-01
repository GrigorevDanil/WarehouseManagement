using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.UnitManagement.Contracts.Requests;

namespace WarehouseManagement.UnitManagement.Application.Queries.GetUnitsWithPaginations;

public record GetUnitsWithPaginationsQuery(
    int Page,
    int PageSize,
    string? Title,
    string? SortDirection,
    bool? IsArchive) : IQuery
{
    public static GetUnitsWithPaginationsQuery Create(GetUnitsWithPaginationsRequest request) => 
        new(request.Page, request.PageSize, request.Title, request.SortDirection, request.IsArchive);
}