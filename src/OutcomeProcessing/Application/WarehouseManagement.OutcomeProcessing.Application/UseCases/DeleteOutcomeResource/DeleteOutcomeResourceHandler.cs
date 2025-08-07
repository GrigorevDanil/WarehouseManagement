using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeResource;

public class DeleteOutcomeResourceHandler : ICommandHandler<Guid,  DeleteOutcomeResourceCommand>
{
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteOutcomeResourceCommand> _validator;
    
    private readonly ILogger<DeleteOutcomeResourceHandler> _logger;

    public DeleteOutcomeResourceHandler(
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository, 
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<DeleteOutcomeResourceCommand> validator, 
        ILogger<DeleteOutcomeResourceHandler> logger)
    {
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteOutcomeResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var outcomeDocumentId = OutcomeDocumentId.Of(command.OutcomeDocumentId);
        
        var outcomeDocumentResult = await _outcomeDocumentRepository.GetByIdAsync(outcomeDocumentId, cancellationToken);

        if (outcomeDocumentResult.IsFailure) 
            return outcomeDocumentResult.Error.ToErrorList();
        
        var outcomeDocument = outcomeDocumentResult.Value;
        
        var outcomeResourceResult = outcomeDocument.GetOutcomeResourceById(command.OutcomeResourceId);
        
        if (outcomeResourceResult.IsFailure) 
            return outcomeResourceResult.Error.ToErrorList();
        
        var outcomeResource = outcomeResourceResult.Value;
        
        outcomeDocument.DeleteResource(outcomeResource);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("OutcomeResource by id `${id}` deleted", command.OutcomeResourceId);
        
        return command.OutcomeResourceId;
    }
}