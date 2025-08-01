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

namespace WarehouseManagement.UnitManagement.Application.UseCases.MoveUnitToArchive;

public class MoveUnitToArchiveHandler : ICommandHandler<Guid, MoveUnitToArchiveCommand>
{
    private readonly IRepository<Unit,UnitId> _unitRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<MoveUnitToArchiveCommand> _validator;
    
    private readonly ILogger<MoveUnitToArchiveHandler> _logger;

    public MoveUnitToArchiveHandler(
        IRepository<Unit, UnitId> unitRepository,
        [FromKeyedServices(Modules.UnitManagement)] IUnitOfWork unitOfWork, 
        IValidator<MoveUnitToArchiveCommand> validator, 
        ILogger<MoveUnitToArchiveHandler> logger)
    {
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(MoveUnitToArchiveCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var unitId = UnitId.Of(command.UnitId);
        
        var unitResult = await _unitRepository.GetByIdAsync(unitId, cancellationToken);
        
        if (unitResult.IsFailure)
            return unitResult.Error.ToErrorList();
        
        var unit = unitResult.Value;
        
        unit.MoveToArchive();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Unit by id `${id}` moved to archive", command.UnitId);
        
        return command.UnitId;
    }
}