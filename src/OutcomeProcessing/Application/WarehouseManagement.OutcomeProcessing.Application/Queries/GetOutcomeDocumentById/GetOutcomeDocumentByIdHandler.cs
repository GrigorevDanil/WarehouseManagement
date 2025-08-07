using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.OutcomeProcessing.Application.Interfaces;
using WarehouseManagement.OutcomeProcessing.Contracts.Responses;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.OutcomeProcessing.Application.Queries.GetOutcomeDocumentById;

public class GetOutcomeDocumentByIdHandler : IQueryHandlerWithResult<OutcomeDocumentResponse, GetOutcomeDocumentByIdQuery>
{
    private readonly IOutcomeProcessingReadDbContext _dbContext;
    
    private readonly IClientManagementContract _clientManagementContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;

    public GetOutcomeDocumentByIdHandler(
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

    public async Task<Result<OutcomeDocumentResponse, ErrorList>> Handle(GetOutcomeDocumentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var outcomeDocument = await _dbContext.OutcomeDocuments
            .Include(x => x.Resources)
            .FirstOrDefaultAsync(x => x.Id == query.OutcomeDocumentId, cancellationToken);
        
        if (outcomeDocument is null) 
            return Errors.General.NotFound(query.OutcomeDocumentId).ToErrorList();
        
        var clientResult = await _clientManagementContract.GetClientById(outcomeDocument.ClientId, cancellationToken);
                
        if (clientResult.IsFailure) 
                return clientResult.Error;
                
        var clientDto = clientResult.Value;
            
        var outcomeResourceResponse = new List<OutcomeResourceResponse>();

        foreach (var outcomeResourceDto in outcomeDocument.Resources)
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

        return new OutcomeDocumentResponse(
            Id: outcomeDocument.Id,
            NumDocument: outcomeDocument.NumDocument,
            Client: new OutcomeDocumentClientResponse(
                Id: clientDto.Id,
                Title: clientDto.Title
            ),
            Status: outcomeDocument.Status,
            CreatedAt: outcomeDocument.CreatedAt,
            Resources: outcomeResourceResponse.ToArray()
        );
    }
}