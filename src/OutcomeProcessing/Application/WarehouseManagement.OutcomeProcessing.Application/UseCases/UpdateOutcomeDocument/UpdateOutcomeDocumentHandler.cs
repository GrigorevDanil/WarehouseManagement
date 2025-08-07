using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.OutcomeProcessing.Contracts;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Domain.ValueObjects;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeDocument;

public class UpdateOutcomeDocumentHandler : ICommandHandler<Guid, UpdateOutcomeDocumentCommand>
{
    private readonly IOutcomeProcessingContract _outcomeProcessingContract;
    
    private readonly IClientManagementContract _clientManagementContract;
    
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateOutcomeDocumentCommand> _validator;
    
    private readonly ILogger<UpdateOutcomeDocumentHandler> _logger;

    public UpdateOutcomeDocumentHandler(
        IOutcomeProcessingContract outcomeProcessingContract,
        IClientManagementContract clientManagementContract,
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository, 
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork, 
        IValidator<UpdateOutcomeDocumentCommand> validator,
        ILogger<UpdateOutcomeDocumentHandler> logger)
    {
        _outcomeProcessingContract = outcomeProcessingContract;
        _clientManagementContract = clientManagementContract;
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateOutcomeDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var outcomeDocumentId = OutcomeDocumentId.Of(command.OutcomeDocumentId);
        
        var outcomeDocumentResult = await _outcomeDocumentRepository.GetByIdAsync(outcomeDocumentId, cancellationToken);

        if (outcomeDocumentResult.IsFailure) 
            return outcomeDocumentResult.Error.ToErrorList();
        
        var outcomeDocument = outcomeDocumentResult.Value;

        if (outcomeDocument.Status == OutcomeDocumentStatus.Signed)
            return Errors.OutcomeDocument.DocumentAlreadySigned().ToErrorList();
        
        var checkClientExistsAndNotArchivedByIdResult = await _clientManagementContract.CheckClientExistsAndNotArchivedById(command.ClientId);
        
        if (checkClientExistsAndNotArchivedByIdResult.IsFailure) 
            return checkClientExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var clientId = ClientId.Of(command.ClientId);
        
        var checkOutcomeDocumentNumDocumentNotExistsResult = await _outcomeProcessingContract.CheckOutcomeDocumentNumDocumentNotExists(command.NumDocument);
        
        if (checkOutcomeDocumentNumDocumentNotExistsResult.IsFailure) 
            return checkOutcomeDocumentNumDocumentNotExistsResult.Error.ToErrorList();
        
        outcomeDocument.UpdateMainInfo(
            NumDocument.Of(command.NumDocument).Value,
            clientId,
            CreatedAt.Of(command.CreatedAt).Value
            );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("OutcomeDocument by ${id} has been updated", command.OutcomeDocumentId);

        return command.OutcomeDocumentId;
    }
}


















