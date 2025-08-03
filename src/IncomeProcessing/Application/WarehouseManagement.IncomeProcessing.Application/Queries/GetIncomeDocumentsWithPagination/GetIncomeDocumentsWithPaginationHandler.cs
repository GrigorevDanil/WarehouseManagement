using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.Core.Models;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Domain.Responses;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Application.Queries.GetIncomeDocumentsWithPagination;

public class GetIncomeDocumentsWithPaginationHandler : IQueryHandlerWithResult<PagedList<IncomeDocumentResponse>,  GetIncomeDocumentsWithPaginationQuery>
{
    private readonly IIncomeProcessingReadDbContext _dbContext;
    
    private readonly IResourceManagementContract  _resourceManagementContract;
    
    private readonly IUnitManagementContract  _unitManagementContract;

    public GetIncomeDocumentsWithPaginationHandler(
        IIncomeProcessingReadDbContext dbContext, 
        IResourceManagementContract resourceManagementContract, 
        IUnitManagementContract unitManagementContract)
    {
        _dbContext = dbContext;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
    }

    public async Task<Result<PagedList<IncomeDocumentResponse>, ErrorList>> Handle(GetIncomeDocumentsWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var incomeDocumentQuery = _dbContext.IncomeDocuments;
        
        incomeDocumentQuery = incomeDocumentQuery.Include(x => x.Resources);

        var startDate = query.StartDate.Kind == DateTimeKind.Unspecified 
            ? DateTime.SpecifyKind(query.StartDate.Date, DateTimeKind.Utc)
            : query.StartDate.Date.ToUniversalTime();

        var endDate = query.EndDate.Kind == DateTimeKind.Unspecified 
            ? DateTime.SpecifyKind(query.EndDate.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc)
            : query.EndDate.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
        
        incomeDocumentQuery = incomeDocumentQuery
            .Where(doc => 
                doc.CreatedAt >= startDate && 
                doc.CreatedAt <= endDate);
            
        incomeDocumentQuery = incomeDocumentQuery 
            .WhereIf(!string.IsNullOrWhiteSpace(query.NumDocument),
                res => res.NumDocument.Contains(query.NumDocument!));
            
        incomeDocumentQuery = incomeDocumentQuery
            .WhereIf(query.ResourceIds != null!,
                doc => doc.Resources.Any(res => query.ResourceIds!.Contains(res.ResourceId)));
            
        incomeDocumentQuery = incomeDocumentQuery
            .WhereIf(query.UnitIds != null!,
                doc => doc.Resources.Any(res => query.UnitIds!.Contains(res.UnitId)));
            
        incomeDocumentQuery = query.SortDirection?.ToLower() == "desc"
            ? incomeDocumentQuery.OrderByDescending(res => res.NumDocument)
            : incomeDocumentQuery.OrderBy(res => res.NumDocument);
        
        var incomeDocumentResponse = new List<IncomeDocumentResponse>();

        foreach (var incomeDocumentDto in incomeDocumentQuery)
        {
            var incomeResourceResponse = new List<IncomeResourceResponse>();

            foreach (var incomeResourceDto in incomeDocumentDto.Resources)
            {
                var resourceResult = await _resourceManagementContract.GetResourceById(incomeResourceDto.ResourceId, cancellationToken);
                
                if (resourceResult.IsFailure) 
                    return resourceResult.Error;
                
                var resourceDto = resourceResult.Value;
                
                var unitResult = await _unitManagementContract.GetUnitById(incomeResourceDto.UnitId, cancellationToken);
                
                if (unitResult.IsFailure) 
                    return unitResult.Error;
                
                var unitDto = unitResult.Value;
                
                incomeResourceResponse.Add(
                    new IncomeResourceResponse(
                        Id: incomeResourceDto.Id,
                        Resource: new ResourceResponse(
                            Id: resourceDto.Id,
                            Title: resourceDto.Title
                            ),
                        Unit: new UnitResponse(
                            Id: unitDto.Id,
                            Title: unitDto.Title
                            ),
                        ResourceStock: incomeResourceDto.ResourceStock
                        ));
            }
            
            incomeDocumentResponse.Add(
                new IncomeDocumentResponse(
                    Id: incomeDocumentDto.Id,
                    NumDocument: incomeDocumentDto.NumDocument,
                    CreatedAt: incomeDocumentDto.CreatedAt,
                    Resources: incomeResourceResponse.ToArray()
                ));
        }

        return incomeDocumentResponse.ToPagedList(query.Page, query.PageSize);
    }
}