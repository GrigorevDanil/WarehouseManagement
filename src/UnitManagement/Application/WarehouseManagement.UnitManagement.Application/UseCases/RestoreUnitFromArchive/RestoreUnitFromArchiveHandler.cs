using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Domain.Entities;

namespace WarehouseManagement.UnitManagement.Application.UseCases.RestoreUnitFromArchive;

public class RestoreUnitFromArchiveHandler : ICommandHandler<Guid, RestoreUnitFromArchiveCommand>
{
    private readonly IRepository<Unit,UnitId> _unitRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<RestoreUnitFromArchiveCommand> _validator;
    
    private readonly ILogger<RestoreUnitFromArchiveHandler> _logger;

    public RestoreUnitFromArchiveHandler(
        IRepository<Unit, UnitId> unitRepository, 
        [FromKeyedServices(Modules.UnitManagement)] IUnitOfWork unitOfWork,
        IValidator<RestoreUnitFromArchiveCommand> validator, 
        ILogger<RestoreUnitFromArchiveHandler> logger)
    {
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(RestoreUnitFromArchiveCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var unitId = UnitId.Of(command.UnitId);
        
        var unitResult = await _unitRepository.GetByIdAsync(unitId, cancellationToken);
        
        if (unitResult.IsFailure)
            return unitResult.Error.ToErrorList();
        
        var unit = unitResult.Value;
        
        unit.RestoreFromArchive();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Unit by id `${id}` restored from archive", command.UnitId);
        
        return command.UnitId;
    }
}