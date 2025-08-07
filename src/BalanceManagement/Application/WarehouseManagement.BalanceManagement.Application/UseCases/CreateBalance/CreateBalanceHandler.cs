using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.BalanceManagement.Contracts;
using WarehouseManagement.BalanceManagement.Domain.Entities;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.CreateBalance;

public class CreateBalanceHandler : ICommandHandler<Guid, CreateBalanceCommand>
{
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IBalanceManagementContract _balanceManagementContract;
    
    private readonly IRepository<Balance,BalanceId> _balanceRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<CreateBalanceCommand> _validator;
    
    private readonly ILogger<CreateBalanceHandler> _logger;

    public CreateBalanceHandler(
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract,
        IBalanceManagementContract balanceManagementContract,
        IRepository<Balance, BalanceId> balanceRepository,
        [FromKeyedServices(Modules.BalanceManagement)] IUnitOfWork unitOfWork,
        IValidator<CreateBalanceCommand> validator,
        ILogger<CreateBalanceHandler> logger)
    {
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _balanceManagementContract = balanceManagementContract;
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateBalanceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(command.ResourceId);
            
        if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
            return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(command.UnitId);
            
        if (checkUnitExistsAndNotArchivedByIdResult.IsFailure)
            return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();
        
        var checkBalanceResourceIdNotExistsResult = 
            await _balanceManagementContract.CheckBalanceByResourceIdAndUnitIdNotExists(
                command.ResourceId,
                command.UnitId);
        
        if (checkBalanceResourceIdNotExistsResult.IsFailure) 
            return checkBalanceResourceIdNotExistsResult.Error.ToErrorList();
        
        var balance = new Balance(
            ResourceId.Of(command.ResourceId), 
            UnitId.Of(command.UnitId),
            Stock.Of(command.ResourceQuantity).Value
        );
        
        var balanceId = await _balanceRepository.AddAsync(balance, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Balance by ${id} has been created", balanceId.Value);
        
        return balanceId.Value;
    }
}