using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.IncomeProcessing.Contracts;
using WarehouseManagement.OutcomeProcessing.Contracts;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.DeleteResource;

public class DeleteResourceHandler : ICommandHandler<Guid, DeleteResourceCommand>
{
    private readonly IIncomeProcessingContract _incomeProcessingContract;
    
    private readonly IOutcomeProcessingContract _outcomeProcessingContract;
    
    private readonly IRepository<Resource,ResourceId> _resourceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteResourceCommand> _validator;
    
    private readonly ILogger<DeleteResourceHandler> _logger;

    public DeleteResourceHandler(
        IRepository<Resource, ResourceId> resourceRepository,
        [FromKeyedServices(Modules.ResourceManagement)] IUnitOfWork unitOfWork, 
        IValidator<DeleteResourceCommand> validator,
        ILogger<DeleteResourceHandler> logger,
        IIncomeProcessingContract incomeProcessingContract,
        IOutcomeProcessingContract outcomeProcessingContract)
    {
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _incomeProcessingContract = incomeProcessingContract;
        _outcomeProcessingContract = outcomeProcessingContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var resourceId = ResourceId.Of(command.ResourceId);
        
        var resourceResult = await _resourceRepository.GetByIdAsync(resourceId, cancellationToken);
        
        if (resourceResult.IsFailure)
            return resourceResult.Error.ToErrorList();
        
        var resource =  resourceResult.Value;
        
        var checkResourceIdNotUsedInAnyIncomeResourceResult = await _incomeProcessingContract.CheckResourceIdNotUsedInAnyIncomeResource(resourceId);
        
        if (checkResourceIdNotUsedInAnyIncomeResourceResult.IsFailure) 
            return checkResourceIdNotUsedInAnyIncomeResourceResult.Error.ToErrorList();
        
        var checkResourceIdNotUsedInAnyOutcomeResourceResult = await _outcomeProcessingContract.CheckResourceIdNotUsedInAnyOutcomeResource(resourceId);
        
        if (checkResourceIdNotUsedInAnyOutcomeResourceResult.IsFailure) 
            return checkResourceIdNotUsedInAnyOutcomeResourceResult.Error.ToErrorList();
        
        _resourceRepository.Delete(resource);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Resource by id `${id}` deleted", command.ResourceId);
        
        return command.ResourceId;
    }
}