using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.CreateResource;

public class CreateResourceHandler : ICommandHandler<Guid, CreateResourceCommand>
{
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IRepository<Resource,ResourceId> _resourceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<CreateResourceCommand> _validator;
    
    private readonly ILogger<CreateResourceHandler> _logger;

    public CreateResourceHandler(
        IRepository<Resource,ResourceId> resourceRepository,
        IValidator<CreateResourceCommand> validator, 
        ILogger<CreateResourceHandler> logger,
        [FromKeyedServices(Modules.ResourceManagement)] IUnitOfWork unitOfWork,
        IResourceManagementContract resourceManagementContract)
    {
        _resourceRepository = resourceRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _resourceManagementContract = resourceManagementContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkResourceTitleNotExistsResult = await _resourceManagementContract.CheckResourceTitleNotExists(command.Title);

        if (checkResourceTitleNotExistsResult.IsFailure) 
            return checkResourceTitleNotExistsResult.Error.ToErrorList();

        var resource = new Resource(
            Title.Of(command.Title).Value
            );
        
        var resourceId = await _resourceRepository.AddAsync(resource, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Resource named ${title} has been created", command.Title);
        
        return resourceId.Value;
    }
}