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

namespace WarehouseManagement.UnitManagement.Application.UseCases.CreateUnit;

public class CreateUnitHandler : ICommandHandler<Guid, CreateUnitCommand>
{
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<Unit,UnitId> _unitRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<CreateUnitCommand> _validator;
    
    private readonly ILogger<CreateUnitHandler> _logger;

    public CreateUnitHandler(
        IUnitManagementContract unitManagementContract,
        IRepository<Unit, UnitId> unitRepository,
        [FromKeyedServices(Modules.UnitManagement)] IUnitOfWork unitOfWork,
        IValidator<CreateUnitCommand> validator, 
        ILogger<CreateUnitHandler> logger)
    {
        _unitManagementContract = unitManagementContract;
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateUnitCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkUnitTitleNotExistsResult = await _unitManagementContract.CheckUnitTitleNotExists(command.Title);

        if (checkUnitTitleNotExistsResult.IsFailure) 
            return checkUnitTitleNotExistsResult.Error.ToErrorList();

        var unit = new Unit(
            Title.Of(command.Title).Value
        );
        
        var unitId = await _unitRepository.AddAsync(unit, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Unit named ${title} has been created", command.Title);
        
        return unitId.Value;
    }
}