using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.BalanceManagement.Contracts;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.OutcomeProcessing.Contracts;
using WarehouseManagement.OutcomeProcessing.Contracts.Responses;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Domain.Entities;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.CreateOutcomeDocumentWithResources;

public class CreateOutcomeDocumentWithResourcesHandler : ICommandHandler<CreateOutcomeDocumentWithResourcesResponse, CreateOutcomeDocumentWithResourcesCommand>
{
    private readonly IOutcomeProcessingContract _outcomeProcessingContract;
    
    private readonly IBalanceManagementContract _balanceManagementContract;
    
    private readonly IClientManagementContract _clientManagementContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<CreateOutcomeDocumentWithResourcesCommand> _validator;
    
    private readonly ILogger<CreateOutcomeDocumentWithResourcesHandler> _logger;

    public CreateOutcomeDocumentWithResourcesHandler(
        IOutcomeProcessingContract outcomeProcessingContract,
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository, 
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<CreateOutcomeDocumentWithResourcesCommand> validator,
        ILogger<CreateOutcomeDocumentWithResourcesHandler> logger,
        IClientManagementContract clientManagementContract, 
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract, 
        IBalanceManagementContract balanceManagementContract)
    {
        _outcomeProcessingContract = outcomeProcessingContract;
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _clientManagementContract = clientManagementContract;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _balanceManagementContract = balanceManagementContract;
    }

    public async Task<Result<CreateOutcomeDocumentWithResourcesResponse, ErrorList>> Handle(
        CreateOutcomeDocumentWithResourcesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkClientExistsAndNotArchivedByIdResult = await _clientManagementContract.CheckClientExistsAndNotArchivedById(command.ClientId);
        
        if (checkClientExistsAndNotArchivedByIdResult.IsFailure) 
            return checkClientExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var clientId = ClientId.Of(command.ClientId);
        
        var checkOutcomeDocumentNumDocumentNotExistsResult = await _outcomeProcessingContract.CheckOutcomeDocumentNumDocumentNotExists(command.NumDocument);
        
        if (checkOutcomeDocumentNumDocumentNotExistsResult.IsFailure) 
            return checkOutcomeDocumentNumDocumentNotExistsResult.Error.ToErrorList();
        
        var outcomeDocument = new OutcomeDocument(
            NumDocument.Of(command.NumDocument).Value,
            clientId,
            CreatedAt.Of(command.CreatedAt).Value
        );
        
        var outcomeDocumentId = outcomeDocument.Id;

        var outcomeResourceIds = new List<Guid>();

        foreach (var item in command.Items)
        {
            var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(item.ResourceId);

            if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
                return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
            
            var resourceId = ResourceId.Of(item.ResourceId);

            var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(item.UnitId);
            
            if (checkUnitExistsAndNotArchivedByIdResult.IsFailure) 
                return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();

            var unitId = UnitId.Of(item.UnitId);

            var balanceResult =
                await _balanceManagementContract.GetBalanceByResourceIdAndUnitId(item.ResourceId, item.UnitId);
            
            if (balanceResult.IsFailure) 
                return balanceResult.Error.ToErrorList();
            
            var balance = balanceResult.Value;

            if (balance.ResourceStock < item.ResourceQuantity) 
                return Errors.Balance.InsufficientStock().ToErrorList();

            var outcomeResource = new OutcomeResource(
                outcomeDocumentId,
                resourceId,
                unitId,
                Quantity.Of(item.ResourceQuantity).Value
            );
            
            outcomeDocument.AddResource(outcomeResource);
            
            outcomeResourceIds.Add(outcomeResource.Id.Value);
        }
        
        await _outcomeDocumentRepository.AddAsync(outcomeDocument, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Outcome document by ${numDocument} has been created", command.NumDocument);
        
        return 
            new CreateOutcomeDocumentWithResourcesResponse(
                outcomeDocument.Id.Value,
                outcomeResourceIds.ToArray());
    }
}