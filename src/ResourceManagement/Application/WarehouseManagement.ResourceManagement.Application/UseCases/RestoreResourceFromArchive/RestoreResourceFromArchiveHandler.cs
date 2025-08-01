using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.RestoreResourceFromArchive;

public class RestoreResourceFromArchiveHandler : ICommandHandler<Guid,  RestoreResourceFromArchiveCommand>
{
    private readonly IRepository<Resource,ResourceId> _resourceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<RestoreResourceFromArchiveCommand> _validator;
    
    private readonly ILogger<RestoreResourceFromArchiveHandler> _logger;


    public RestoreResourceFromArchiveHandler(
        IRepository<Resource, ResourceId> resourceRepository,
        [FromKeyedServices(Modules.ResourceManagement)] IUnitOfWork unitOfWork, 
        IValidator<RestoreResourceFromArchiveCommand> validator, 
        ILogger<RestoreResourceFromArchiveHandler> logger)
    {
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(RestoreResourceFromArchiveCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var resourceId = ResourceId.Of(command.ResourceId);
        
        var resourceResult = await _resourceRepository.GetByIdAsync(resourceId, cancellationToken);
        
        if (resourceResult.IsFailure)
            return resourceResult.Error.ToErrorList();
        
        var resource = resourceResult.Value;
        
        resource.RestoreFromArchive();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Resource by id `${id}` restored from archive", command.ResourceId);
        
        return command.ResourceId;
    }
}