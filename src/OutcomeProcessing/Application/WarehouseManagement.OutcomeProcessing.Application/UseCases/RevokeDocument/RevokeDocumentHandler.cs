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

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.RevokeDocument;

public class RevokeDocumentHandler : ICommandHandler<Guid, RevokeDocumentCommand>
{
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;

    private readonly IOutboxRepository _outboxRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<RevokeDocumentCommand> _validator;
    
    private readonly ILogger<RevokeDocumentHandler> _logger;

    public RevokeDocumentHandler(
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract,
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository,
        [FromKeyedServices(Modules.OutcomeProcessing)] IOutboxRepository outboxRepository,
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork, 
        IValidator<RevokeDocumentCommand> validator,
        ILogger<RevokeDocumentHandler> logger)
    {
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(RevokeDocumentCommand command, CancellationToken cancellationToken = default)
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
            var checkResourceExistsByIdResult = await _resourceManagementContract.CheckResourceExistsById(outcomeResource.ResourceId.Value);
            
            if (checkResourceExistsByIdResult.IsFailure)
                return checkResourceExistsByIdResult.Error.ToErrorList();
            
            var checkUnitExistsByIdResult = await _unitManagementContract.CheckUnitExistsById(outcomeResource.UnitId.Value);
            
            if (checkUnitExistsByIdResult.IsFailure)
                return checkUnitExistsByIdResult.Error.ToErrorList();
            
            var @event = new RevokeDocumentEvent(
                outcomeResource.ResourceId.Value,
                outcomeResource.UnitId.Value,
                outcomeResource.ResourceQuantity.Value
            );
        
            await _outboxRepository.AddAsync(@event, cancellationToken);
        }
        
        var revokeDocumentResult = outcomeDocument.RevokeDocument();
        
        if (revokeDocumentResult.IsFailure) 
            return revokeDocumentResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("OutcomeDocument by ${id} has been revoked", command.OutcomeDocumentId);

        return command.OutcomeDocumentId;
    }
}