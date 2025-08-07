using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.BalanceManagement.Application.Interfaces;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.ReduceResourceStockBalance;

public class ReduceResourceStockBalanceHandler : ICommandHandler<Guid, ReduceResourceStockBalanceCommand>
{
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IBalanceRepository _balanceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<ReduceResourceStockBalanceCommand> _validator;
    
    private readonly ILogger<ReduceResourceStockBalanceHandler> _logger;
    
    public ReduceResourceStockBalanceHandler(
        IResourceManagementContract resourceManagementContract, 
        IUnitManagementContract unitManagementContract,
        IBalanceRepository balanceRepository, 
        [FromKeyedServices(Modules.BalanceManagement)] IUnitOfWork unitOfWork,
        IValidator<ReduceResourceStockBalanceCommand> validator,
        ILogger<ReduceResourceStockBalanceHandler> logger)
    {
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(ReduceResourceStockBalanceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(command.ResourceId);
            
        if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
            return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var resourceId = ResourceId.Of(command.ResourceId);
        
        var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(command.UnitId);
            
        if (checkUnitExistsAndNotArchivedByIdResult.IsFailure)
            return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var unitId = UnitId.Of(command.UnitId);
        
        var balanceResult = await _balanceRepository.GetBalanceByResourceIdAndUnitIdAsync(resourceId, unitId, cancellationToken);
        
        if (balanceResult.IsFailure)
            return balanceResult.Error.ToErrorList();
        
        var balance =  balanceResult.Value;

        if (balance.ResourceStock.Value == command.SubtractedResourceQuantity) 
            _balanceRepository.Delete(balance);
        else 
            balance.ReduceResourceStock(
                command.SubtractedResourceQuantity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("ResourceStock Balance by id `{id}` reduced", balance.Id.Value);
        
        return balance.Id.Value;
    }
}