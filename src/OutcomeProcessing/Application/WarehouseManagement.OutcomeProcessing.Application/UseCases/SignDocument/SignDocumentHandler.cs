using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.BalanceManagement.Contracts;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.OutcomeProcessing.Contracts.Messaging;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.SignDocument;

public class SignDocumentHandler : ICommandHandler<Guid,  SignDocumentCommand>
{
    private readonly IBalanceManagementContract _balanceManagementContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;

    private readonly IOutboxRepository _outboxRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<SignDocumentCommand> _validator;
    
    private readonly ILogger<SignDocumentHandler> _logger;

    public SignDocumentHandler(
        IBalanceManagementContract balanceManagementContract, 
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository,
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<SignDocumentCommand> validator, 
        ILogger<SignDocumentHandler> logger, 
        [FromKeyedServices(Modules.OutcomeProcessing)] IOutboxRepository outboxRepository, 
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract)
    {
        _balanceManagementContract = balanceManagementContract;
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _outboxRepository = outboxRepository;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(SignDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var outcomeDocumentId = OutcomeDocumentId.Of(command.OutcomeDocumentId);
        
        var outcomeDocumentResult = await _outcomeDocumentRepository.GetByIdAsync(outcomeDocumentId, cancellationToken);

        if (outcomeDocumentResult.IsFailure) 
            return outcomeDocumentResult.Error.ToErrorList();
        
        var outcomeDocument = outcomeDocumentResult.Value;
        
        foreach (var outcomeResource in outcomeDocument.Resources)
        {
            var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(outcomeResource.ResourceId.Value);
            
            if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
                return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
            
            var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(outcomeResource.UnitId.Value);
            
            if (checkUnitExistsAndNotArchivedByIdResult.IsFailure)
                return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();
            
            var balanceResult = await _balanceManagementContract.GetBalanceByResourceIdAndUnitId(
                outcomeResource.ResourceId.Value,
                outcomeResource.UnitId.Value);
            
            if (balanceResult.IsFailure) 
                return balanceResult.Error.ToErrorList();
            
            var balance = balanceResult.Value;

            if (balance.ResourceStock < outcomeResource.ResourceQuantity.Value)
                return Errors.Balance.InsufficientStock().ToErrorList();
            
            var @event = new SignDocumentEvent(
                outcomeResource.ResourceId.Value,
                outcomeResource.UnitId.Value,
                outcomeResource.ResourceQuantity.Value
            );
        
            await _outboxRepository.AddAsync(@event, cancellationToken);
        }
        
        var signDocumentResult = outcomeDocument.SignDocument();
        
        if (signDocumentResult.IsFailure) 
            return signDocumentResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("OutcomeDocument by ${id} has been signed", command.OutcomeDocumentId);

        return command.OutcomeDocumentId;
    }
}