using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;

namespace WarehouseManagement.IncomeProcessing.Application.Queries.GetIncomeDocumentsWithPagination;

public record GetIncomeDocumentsWithPaginationQuery(
    int Page,
    int PageSize,
    DateTime StartDate,
    DateTime EndDate,
    string? NumDocument,
    Guid?[] ResourceIds,
    Guid?[] UnitIds,
    string? SortDirection) : IQuery
{
    public static GetIncomeDocumentsWithPaginationQuery Create(GetIncomeDocumentsWithPaginationRequest request) => 
        new(
            request.Page, 
            request.PageSize, 
            request.StartDate,
            request.EndDate,
            request.NumDocument,
            request.ResourceIds, 
            request.UnitIds,
            request.SortDirection);
}