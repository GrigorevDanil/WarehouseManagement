using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.ResourceManagement.Application.UseCases.CreateResource;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.UpdateResource;

public class UpdateResourceHandler : ICommandHandler<Guid,  UpdateResourceCommand>
{
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IRepository<Resource,ResourceId> _resourceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateResourceCommand> _validator;
    
    private readonly ILogger<UpdateResourceHandler> _logger;

    public UpdateResourceHandler(
        IResourceManagementContract resourceManagementContract,
        IRepository<Resource, ResourceId> resourceRepository,
        [FromKeyedServices(Modules.ResourceManagement)] IUnitOfWork unitOfWork,
        IValidator<UpdateResourceCommand> validator, 
        ILogger<UpdateResourceHandler> logger)
    {
        _resourceManagementContract = resourceManagementContract;
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var resourceId = ResourceId.Of(command.ResourceId);
        
        var resourceResult = await _resourceRepository.GetByIdAsync(resourceId, cancellationToken);
        
        if (resourceResult.IsFailure)
            return resourceResult.Error.ToErrorList();
        
        var resource =  resourceResult.Value;
        
        if (resource.Title.Value != command.Title)
        {
            var checkResourceTitleNotExistsResult = await _resourceManagementContract.CheckResourceTitleNotExists(command.Title);
            
            if (checkResourceTitleNotExistsResult.IsFailure) 
                return checkResourceTitleNotExistsResult.Error.ToErrorList();
        }

        resource.UpdateMainInfo(Title.Of(command.Title).Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Resource by id `${id}` has updated", command.ResourceId);
        
        return resourceId.Value;
    }
}