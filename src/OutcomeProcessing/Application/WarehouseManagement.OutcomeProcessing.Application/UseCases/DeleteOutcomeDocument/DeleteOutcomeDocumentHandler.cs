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

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeDocument;

public class DeleteOutcomeDocumentHandler : ICommandHandler<Guid, DeleteOutcomeDocumentCommand>
{
    private readonly IRepository<OutcomeDocument,OutcomeDocumentId> _outcomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteOutcomeDocumentCommand> _validator;
    
    private readonly ILogger<DeleteOutcomeDocumentHandler> _logger;

    public DeleteOutcomeDocumentHandler(
        IRepository<OutcomeDocument, OutcomeDocumentId> outcomeDocumentRepository, 
        [FromKeyedServices(Modules.OutcomeProcessing)] IUnitOfWork unitOfWork, 
        IValidator<DeleteOutcomeDocumentCommand> validator,
        ILogger<DeleteOutcomeDocumentHandler> logger)
    {
        _outcomeDocumentRepository = outcomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteOutcomeDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var outcomeDocumentId = OutcomeDocumentId.Of(command.OutcomeDocumentId);
        
        var outcomeDocumentResult = await _outcomeDocumentRepository.GetByIdAsync(outcomeDocumentId, cancellationToken);

        if (outcomeDocumentResult.IsFailure) 
            return outcomeDocumentResult.Error.ToErrorList();
        
        var outcomeDocument = outcomeDocumentResult.Value;
        
        _outcomeDocumentRepository.Delete(outcomeDocument);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("OutcomeDocument by id `${id}` deleted", command.OutcomeDocumentId);
        
        return command.OutcomeDocumentId;
    }
}