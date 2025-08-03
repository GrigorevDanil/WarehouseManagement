using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;

namespace WarehouseManagement.IncomeProcessing.Application.Queries.GetIncomeDocumentById;

public record GetIncomeDocumentByIdQuery(
    Guid IncomeDocumentId,
    Guid?[] ResourceIds,
    Guid?[] UnitIds) : IQuery
{
    public static GetIncomeDocumentByIdQuery Create(Guid incomeDocumentId, GetIncomeDocumentByIdRequest request) => 
        new(
            incomeDocumentId,
            request.ResourceIds, 
            request.UnitIds);
}