using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;
using WarehouseManagement.UnitManagement.Domain.Entities;

namespace WarehouseManagement.UnitManagement.Application.UseCases.UpdateUnit;

public class UpdateUnitHandler : ICommandHandler<Guid, UpdateUnitCommand>
{
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<Unit,UnitId> _unitRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateUnitCommand> _validator;
    
    private readonly ILogger<UpdateUnitHandler> _logger;

    public UpdateUnitHandler(
        IUnitManagementContract unitManagementContract, 
        IRepository<Unit, UnitId> unitRepository, 
        [FromKeyedServices(Modules.UnitManagement)] IUnitOfWork unitOfWork,
        IValidator<UpdateUnitCommand> validator, 
        ILogger<UpdateUnitHandler> logger)
    {
        _unitManagementContract = unitManagementContract;
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateUnitCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var unitId = UnitId.Of(command.UnitId);
        
        var unitResult = await _unitRepository.GetByIdAsync(unitId, cancellationToken);

        if (unitResult.IsFailure)     
            return unitResult.Error.ToErrorList();
        
        var unit = unitResult.Value;
        
        if (unit.Title.Value != command.Title)
        {
            var checkUnitTitleNotExistsResult = await _unitManagementContract.CheckUnitTitleNotExists(command.Title);
            
            if (checkUnitTitleNotExistsResult.IsFailure) 
                return checkUnitTitleNotExistsResult.Error.ToErrorList();
        }
        
        unit.UpdateMainInfo(
            Title.Of(command.Title).Value
        );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Unit by id `${id}` has updated", command.UnitId);
        
        return command.UnitId;
    }
}