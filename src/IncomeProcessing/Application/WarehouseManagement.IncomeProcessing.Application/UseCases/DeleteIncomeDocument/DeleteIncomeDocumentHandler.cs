using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.IncomeProcessing.Contracts.Messaging;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeDocument;

public class DeleteIncomeDocumentHandler : ICommandHandler<Guid, DeleteIncomeDocumentCommand>
{
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IOutboxRepository _outboxRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteIncomeDocumentCommand> _validator;
    
    private readonly ILogger<DeleteIncomeDocumentHandler> _logger;

    public DeleteIncomeDocumentHandler(
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository, 
        [FromKeyedServices(Modules.IncomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<DeleteIncomeDocumentCommand> validator,
        ILogger<DeleteIncomeDocumentHandler> logger,
        [FromKeyedServices(Modules.IncomeProcessing)] IOutboxRepository outboxRepository)
    {
        _incomeDocumentRepository = incomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _outboxRepository = outboxRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteIncomeDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var incomeDocumentId = IncomeDocumentId.Of(command.IncomeDocumentId);
        
        var incomeDocumentResult = await _incomeDocumentRepository.GetByIdAsync(incomeDocumentId, cancellationToken);
        
        if (incomeDocumentResult.IsFailure)
            return incomeDocumentResult.Error.ToErrorList();
        
        var incomeDocument =  incomeDocumentResult.Value;
        
        _incomeDocumentRepository.Delete(incomeDocument);

        foreach (var incomeResource in incomeDocument.Resources)
        {
            var @event = new DeleteIncomeResourceEvent(
                incomeResource.ResourceId.Value,
                incomeResource.UnitId.Value,
                incomeResource.ResourceStock.Value
            );

            await _outboxRepository.AddAsync(@event, cancellationToken);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("IncomeDocument by id `${id}` deleted", command.IncomeDocumentId);
        
        return command.IncomeDocumentId;
    }
}