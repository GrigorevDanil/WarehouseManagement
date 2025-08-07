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
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Domain.Entities;

namespace WarehouseManagement.UnitManagement.Application.UseCases.DeleteUnit;

public class DeleteUnitHandler : ICommandHandler<Guid,  DeleteUnitCommand>
{
    private readonly IIncomeProcessingContract _incomeProcessingContract;
    
    private readonly IOutcomeProcessingContract _outcomeProcessingContract;
    
    private readonly IRepository<Unit,UnitId> _unitRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteUnitCommand> _validator;
    
    private readonly ILogger<DeleteUnitHandler> _logger;

    public DeleteUnitHandler(
        IRepository<Unit, UnitId> unitRepository, 
        [FromKeyedServices(Modules.UnitManagement)] IUnitOfWork unitOfWork,
        IValidator<DeleteUnitCommand> validator,
        ILogger<DeleteUnitHandler> logger,
        IIncomeProcessingContract incomeProcessingContract,
        IOutcomeProcessingContract outcomeProcessingContract)
    {
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _incomeProcessingContract = incomeProcessingContract;
        _outcomeProcessingContract = outcomeProcessingContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteUnitCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var unitId = UnitId.Of(command.UnitId);
        
        var unitResult = await _unitRepository.GetByIdAsync(unitId, cancellationToken);
        
        if (unitResult.IsFailure)
            return unitResult.Error.ToErrorList();
        
        var unit =  unitResult.Value;
        
        var checkUnitIdNotUsedInAnyIncomeResourceResult = await _incomeProcessingContract.CheckUnitIdNotUsedInAnyIncomeResource(unitId);
        
        if (checkUnitIdNotUsedInAnyIncomeResourceResult.IsFailure) 
            return checkUnitIdNotUsedInAnyIncomeResourceResult.Error.ToErrorList();

        var checkUnitIdNotUsedInAnyOutcomeResourceResult = await _outcomeProcessingContract.CheckUnitIdNotUsedInAnyOutcomeResource(unitId);
        
        if (checkUnitIdNotUsedInAnyOutcomeResourceResult.IsFailure) 
            return checkUnitIdNotUsedInAnyOutcomeResourceResult.Error.ToErrorList();
        
        _unitRepository.Delete(unit);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Unit by id `${id}` deleted", command.UnitId);
        
        return command.UnitId;
    }
}