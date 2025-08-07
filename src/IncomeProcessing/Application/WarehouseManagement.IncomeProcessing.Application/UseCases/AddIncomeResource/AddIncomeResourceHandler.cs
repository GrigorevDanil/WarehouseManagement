using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.IncomeProcessing.Contracts;
using WarehouseManagement.IncomeProcessing.Contracts.Messaging;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.IncomeProcessing.Domain.Entities;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.AddIncomeResource;

public class AddIncomeResourceHandler : ICommandHandler<Guid, AddIncomeResourceCommand>
{
    private readonly IIncomeProcessingContract _incomeProcessingContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IOutboxRepository _outboxRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<AddIncomeResourceCommand> _validator;
    
    private readonly ILogger<AddIncomeResourceHandler> _logger;

    public AddIncomeResourceHandler(
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract, 
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository, 
        [FromKeyedServices(Modules.IncomeProcessing)] IOutboxRepository outboxRepository,
        [FromKeyedServices(Modules.IncomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<AddIncomeResourceCommand> validator, 
        ILogger<AddIncomeResourceHandler> logger, 
        IIncomeProcessingContract incomeProcessingContract)
    {
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _incomeDocumentRepository = incomeDocumentRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _incomeProcessingContract = incomeProcessingContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddIncomeResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkIncomeResourceExistsInDocumentResult = await _incomeProcessingContract.CheckIncomeResourceExistsInDocument(command.IncomeDocumentId, command.ResourceId);

        if (checkIncomeResourceExistsInDocumentResult.IsFailure)
            return checkIncomeResourceExistsInDocumentResult.Error.ToErrorList();

        var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(command.ResourceId);

        if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
            return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(command.UnitId);
        
        if (checkUnitExistsAndNotArchivedByIdResult.IsFailure)
            return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();

        var incomeDocumentId = IncomeDocumentId.Of(command.IncomeDocumentId);
        
        var incomeDocumentResult = await _incomeDocumentRepository.GetByIdAsync(incomeDocumentId,cancellationToken);
        
        if (incomeDocumentResult.IsFailure) 
            return incomeDocumentResult.Error.ToErrorList();
        
        var incomeDocument = incomeDocumentResult.Value;
        
        var incomeResource = new IncomeResource(
            incomeDocumentId,
            ResourceId.Of(command.ResourceId),
            UnitId.Of(command.UnitId),
            Quantity.Of(command.ResourceQuantity).Value
            );
        
        incomeDocument.AddResource(incomeResource);

        var @event = new AddIncomeResourceEvent(
            incomeResource.ResourceId.Value,
            incomeResource.UnitId.Value,
            incomeResource.ResourceQuantity.Value
            );
        
        await _outboxRepository.AddAsync(@event, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("IncomeResource by {incomeResourceId} has been added to IncomeDocument by {incomeDocumentId}",
            incomeResource.Id.Value, 
            command.IncomeDocumentId);
        
        return incomeResource.Id.Value;
    }
}