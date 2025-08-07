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
using WarehouseManagement.IncomeProcessing.Contracts.Messaging;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeResource;

public class UpdateIncomeResourceHandler : ICommandHandler<Guid, UpdateIncomeResourceCommand>
{
    private readonly IBalanceManagementContract _balanceManagementContract;
    
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IOutboxRepository _outboxRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateIncomeResourceCommand> _validator;
    
    private readonly ILogger<UpdateIncomeResourceHandler> _logger;


    public UpdateIncomeResourceHandler(
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract, 
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository, 
        [FromKeyedServices(Modules.IncomeProcessing)]  IUnitOfWork unitOfWork, 
        IValidator<UpdateIncomeResourceCommand> validator, 
        ILogger<UpdateIncomeResourceHandler> logger, 
        [FromKeyedServices(Modules.IncomeProcessing)] IOutboxRepository outboxRepository,
        IBalanceManagementContract balanceManagementContract)
    {
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _incomeDocumentRepository = incomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _outboxRepository = outboxRepository;
        _balanceManagementContract = balanceManagementContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateIncomeResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var incomeDocumentId = IncomeDocumentId.Of(command.IncomeDocumentId);
        
        var incomeDocumentResult = await _incomeDocumentRepository.GetByIdAsync(incomeDocumentId, cancellationToken);
        
        if (incomeDocumentResult.IsFailure) 
            return incomeDocumentResult.Error.ToErrorList();
        
        var incomeDocument = incomeDocumentResult.Value;

        var incomeResourceResult = incomeDocument.GetIncomeResourceById(command.IncomeResourceId);
        
        if (incomeResourceResult.IsFailure) 
            return incomeResourceResult.Error.ToErrorList();
        
        var incomeResource = incomeResourceResult.Value;

        if (incomeResource.ResourceId.Value != command.ResourceId)
        {
            var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(command.ResourceId);

            if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
                return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
        }

        if (incomeResource.UnitId.Value != command.UnitId)
        {
            var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(command.UnitId);
        
            if (checkUnitExistsAndNotArchivedByIdResult.IsFailure)
                return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();
        }
        
        var balanceResult = await _balanceManagementContract.GetBalanceByResourceIdAndUnitId(command.ResourceId, command.UnitId);
        
        if (balanceResult.IsFailure)
            return balanceResult.Error.ToErrorList();
        
        var balance = balanceResult.Value;
        
        var quantity = command.ResourceQuantity - incomeResource.ResourceQuantity.Value;
        
        var balanceResourceStock = balance.ResourceStock + quantity;
        
        if (balanceResourceStock < 0) 
            return Errors.Balance.InsufficientStock().ToErrorList();
        
        var @event = new UpdateIncomeResourceEvent(
            command.ResourceId,
            command.UnitId,
            quantity
        );
        
        await _outboxRepository.AddAsync(@event, cancellationToken);
        
        incomeResource.UpdateMainInfo(
            ResourceId.Of(command.ResourceId), 
            UnitId.Of(command.UnitId), 
            Quantity.Of(command.ResourceQuantity).Value
            );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("IncomeResource by ${id} has been updated", command.IncomeResourceId);
        
        return command.IncomeDocumentId;
    }
}