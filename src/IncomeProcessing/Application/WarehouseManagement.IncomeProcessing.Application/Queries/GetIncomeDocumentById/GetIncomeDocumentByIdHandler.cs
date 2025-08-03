using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Domain.Responses;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Application.Queries.GetIncomeDocumentById;

public class GetIncomeDocumentByIdHandler : IQueryHandlerWithResult<IncomeDocumentResponse, GetIncomeDocumentByIdQuery>
{
    private readonly IIncomeProcessingReadDbContext _dbContext;
    
    private readonly IResourceManagementContract  _resourceManagementContract;
    
    private readonly IUnitManagementContract  _unitManagementContract;

    public GetIncomeDocumentByIdHandler(
        IIncomeProcessingReadDbContext dbContext, 
        IResourceManagementContract resourceManagementContract, 
        IUnitManagementContract unitManagementContract)
    {
        _dbContext = dbContext;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
    }

    public async Task<Result<IncomeDocumentResponse, ErrorList>> Handle(GetIncomeDocumentByIdQuery query,
    CancellationToken cancellationToken = default)
{
    var incomeDocumentQuery = _dbContext.IncomeDocuments;

    incomeDocumentQuery = incomeDocumentQuery.Include(x => x.Resources);

    var incomeDocumentDto =
        await incomeDocumentQuery.FirstOrDefaultAsync(x => x.Id == query.IncomeDocumentId, cancellationToken);

    if (incomeDocumentDto == null)
        return Errors.General.NotFound(query.IncomeDocumentId).ToErrorList();

    var incomeResourceResponse = new List<IncomeResourceResponse>();
    
    var filteredResources = incomeDocumentDto.Resources.AsEnumerable();

    if (query.ResourceIds != null!) 
        filteredResources = filteredResources.Where(res => query.ResourceIds.Contains(res.ResourceId));

    if (query.UnitIds != null!) 
        filteredResources = filteredResources.Where(res => query.UnitIds.Contains(res.UnitId));

    foreach (var incomeResourceDto in filteredResources)
    {
        var resourceResult =
            await _resourceManagementContract.GetResourceById(incomeResourceDto.ResourceId, cancellationToken);

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

    var incomeDocumentResponse = new IncomeDocumentResponse(
        Id: incomeDocumentDto.Id,
        NumDocument: incomeDocumentDto.NumDocument,
        CreatedAt: incomeDocumentDto.CreatedAt,
        Resources: incomeResourceResponse.ToArray()
    );

    return incomeDocumentResponse;
}
}