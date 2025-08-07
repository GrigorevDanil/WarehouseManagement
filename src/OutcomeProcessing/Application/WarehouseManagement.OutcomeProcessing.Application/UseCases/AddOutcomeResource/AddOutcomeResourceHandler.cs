using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.BalanceManagement.Contracts;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Domain.Entities;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.AddOutcomeResource;

public class AddOutcomeResourceHandler : ICommandHandler<Guid, AddOutcomeResourceCommand>
{
    private readonly IBalanceManagementContract _balanceManagementContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<AddOutcomeResourceCommand> _validator;
    
    private readonly ILogger<AddOutcomeResourceHandler> _logger;


    public AddOutcomeResourceHandler(
        IBalanceManagementContract balanceManagementContract,
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract, 
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository, 
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<AddOutcomeResourceCommand> validator,
        ILogger<AddOutcomeResourceHandler> logger)
    {
        _balanceManagementContract = balanceManagementContract;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddOutcomeResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var outcomeDocumentId = OutcomeDocumentId.Of(command.OutcomeDocumentId);
        
        var outcomeDocumentResult = await _outcomeDocumentRepository.GetByIdAsync(outcomeDocumentId, cancellationToken);

        if (outcomeDocumentResult.IsFailure) 
            return outcomeDocumentResult.Error.ToErrorList();
        
        var outcomeDocument = outcomeDocumentResult.Value;
        
        var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(command.ResourceId);

        if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
            return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
            
        var resourceId = ResourceId.Of(command.ResourceId);

        var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(command.UnitId);
            
        if (checkUnitExistsAndNotArchivedByIdResult.IsFailure) 
            return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();

        var unitId = UnitId.Of(command.UnitId);

        var balanceResult =
            await _balanceManagementContract.GetBalanceByResourceIdAndUnitId(command.ResourceId, command.UnitId);
            
        if (balanceResult.IsFailure) 
            return balanceResult.Error.ToErrorList();
            
        var balance = balanceResult.Value;

        if (balance.ResourceStock < command.ResourceQuantity) 
            return Errors.Balance.InsufficientStock().ToErrorList();

        var outcomeResource = new OutcomeResource(
            outcomeDocumentId,
            resourceId,
            unitId,
            Quantity.Of(command.ResourceQuantity).Value
        );
            
        outcomeDocument.AddResource(outcomeResource);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("OutcomeResource by {outcomeResourceId} has been added to OutcomeDocument by {outcomeDocumentId}",
            outcomeResource.Id.Value, 
            command.OutcomeDocumentId);
        
        return outcomeResource.Id.Value;
    }
}