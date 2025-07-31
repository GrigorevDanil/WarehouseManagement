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

namespace WarehouseManagement.ResourceManagement.Application.UseCases.MoveResourceToArchive;

public class MoveResourceToArchiveHandler : ICommandHandler<Guid, MoveResourceToArchiveCommand>
{
    private readonly IRepository<Resource,ResourceId> _resourceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<MoveResourceToArchiveCommand> _validator;
    
    private readonly ILogger<MoveResourceToArchiveHandler> _logger;
    
    public MoveResourceToArchiveHandler(
        IRepository<Resource, ResourceId> resourceRepository,
        [FromKeyedServices(Modules.ResourceManagement)] IUnitOfWork unitOfWork,
        IValidator<MoveResourceToArchiveCommand> validator,
        ILogger<MoveResourceToArchiveHandler> logger)
    {
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(MoveResourceToArchiveCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var resourceId = ResourceId.Of(command.ResourceId);
        
        var resourceResult = await _resourceRepository.GetByIdAsync(resourceId, cancellationToken);
        
        if (resourceResult.IsFailure)
            return resourceResult.Error.ToErrorList();
        
        var resource = resourceResult.Value;
        
        resource.MoveToArchive();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Resource by id `${title}` moved to archive", command.ResourceId);
        
        return command.ResourceId;
    }
}