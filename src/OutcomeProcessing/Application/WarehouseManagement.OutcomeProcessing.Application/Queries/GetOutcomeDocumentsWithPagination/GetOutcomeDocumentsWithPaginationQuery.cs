using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.OutcomeProcessing.Contracts.Requests;

namespace WarehouseManagement.OutcomeProcessing.Application.Queries.GetOutcomeDocumentsWithPagination;

public record GetOutcomeDocumentsWithPaginationQuery(
    int Page,
    int PageSize,
    DateTime StartDate,
    DateTime EndDate,
    string? NumDocument,
    Guid?[] ClientIds,
    Guid?[] ResourceIds,
    Guid?[] UnitIds,
    string? SortDirection) : IQuery
{
    public static GetOutcomeDocumentsWithPaginationQuery Create(GetOutcomeDocumentsWithPaginationRequest request) => 
        new(
            request.Page, 
            request.PageSize, 
            request.StartDate,
            request.EndDate,
            request.NumDocument,
            request.ClientIds, 
            request.ResourceIds, 
            request.UnitIds,
            request.SortDirection);
}