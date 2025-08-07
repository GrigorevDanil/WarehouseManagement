using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.Core.Models;
using WarehouseManagement.OutcomeProcessing.Application.Interfaces;
using WarehouseManagement.OutcomeProcessing.Contracts.Responses;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.OutcomeProcessing.Application.Queries.GetOutcomeDocumentsWithPagination;

public class GetOutcomeDocumentsWithPaginationHandler : IQueryHandlerWithResult<PagedList<OutcomeDocumentResponse>, GetOutcomeDocumentsWithPaginationQuery>
{
    private readonly IOutcomeProcessingReadDbContext _dbContext;
    
    private readonly IClientManagementContract _clientManagementContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;

    public GetOutcomeDocumentsWithPaginationHandler(
        IOutcomeProcessingReadDbContext dbContext,
        IClientManagementContract clientManagementContract, 
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract)
    {
        _dbContext = dbContext;
        _clientManagementContract = clientManagementContract;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
    }

    public async Task<Result<PagedList<OutcomeDocumentResponse>, ErrorList>> Handle(GetOutcomeDocumentsWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var outcomeDocumentQuery = _dbContext.OutcomeDocuments;
        
        outcomeDocumentQuery = outcomeDocumentQuery.Include(x => x.Resources);

        var startDate = query.StartDate.Kind == DateTimeKind.Unspecified 
            ? DateTime.SpecifyKind(query.StartDate.Date, DateTimeKind.Utc)
            : query.StartDate.Date.ToUniversalTime();

        var endDate = query.EndDate.Kind == DateTimeKind.Unspecified 
            ? DateTime.SpecifyKind(query.EndDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc)
            : query.EndDate.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
            
        outcomeDocumentQuery = outcomeDocumentQuery 
            .Where(doc => 
                doc.CreatedAt >= startDate && 
                doc.CreatedAt <= endDate)        
            .WhereIf(!string.IsNullOrWhiteSpace(query.NumDocument),
                res => res.NumDocument.Contains(query.NumDocument!))
            .WhereIf(query.ClientIds != null!,
                doc => query.ClientIds!.Contains(doc.ClientId)) 
            .WhereIf(query.ResourceIds != null!,
                doc => doc.Resources.Any(res => query.ResourceIds!.Contains(res.ResourceId)))
            .WhereIf(query.UnitIds != null!,
                doc => doc.Resources.Any(res => query.UnitIds!.Contains(res.UnitId)));
        
        outcomeDocumentQuery = query.SortDirection?.ToLower() == "desc"
            ? outcomeDocumentQuery.OrderByDescending(res => res.NumDocument)
            : outcomeDocumentQuery.OrderBy(res => res.NumDocument);
        
        var outcomeDocumentResponse = new List<OutcomeDocumentResponse>();

        foreach (var outcomeDocumentDto in outcomeDocumentQuery)
        {
            var clientResult = await _clientManagementContract.GetClientById(outcomeDocumentDto.ClientId, cancellationToken);
                
            if (clientResult.IsFailure) 
                return clientResult.Error;
                
            var clientDto = clientResult.Value;
            
            var outcomeResourceResponse = new List<OutcomeResourceResponse>();

            foreach (var outcomeResourceDto in outcomeDocumentDto.Resources)
            {
                var resourceResult = await _resourceManagementContract.GetResourceById(outcomeResourceDto.ResourceId, cancellationToken);
                
                if (resourceResult.IsFailure) 
                    return resourceResult.Error;
                
                var resourceDto = resourceResult.Value;
                
                var unitResult = await _unitManagementContract.GetUnitById(outcomeResourceDto.UnitId, cancellationToken);
                
                if (unitResult.IsFailure) 
                    return unitResult.Error;
                
                var unitDto = unitResult.Value;
                
                outcomeResourceResponse.Add(
                    new OutcomeResourceResponse(
                        Id: outcomeResourceDto.Id,
                        Resource: new OutcomeResourceResourceResponse(
                            Id: resourceDto.Id,
                            Title: resourceDto.Title
                            ),
                        Unit: new OutcomeResourceUnitResponse(
                            Id: unitDto.Id,
                            Title: unitDto.Title
                            ),
                        ResourceQuantity: outcomeResourceDto.ResourceQuantity
                        ));
            }
            
            outcomeDocumentResponse.Add(
                new OutcomeDocumentResponse(
                    Id: outcomeDocumentDto.Id,
                    NumDocument: outcomeDocumentDto.NumDocument,
                    Client: new OutcomeDocumentClientResponse(
                        Id: clientDto.Id,
                        Title: clientDto.Title
                        ),
                    Status: outcomeDocumentDto.Status,
                    CreatedAt: outcomeDocumentDto.CreatedAt,
                    Resources: outcomeResourceResponse.ToArray()
                ));
        }

        return outcomeDocumentResponse.ToPagedList(query.Page, query.PageSize);
    }
}