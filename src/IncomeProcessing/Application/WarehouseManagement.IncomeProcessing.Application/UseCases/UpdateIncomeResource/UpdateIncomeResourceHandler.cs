using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeDocument;
using WarehouseManagement.IncomeProcessing.Contracts;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeResource;

public class UpdateIncomeResourceHandler : ICommandHandler<Guid, UpdateIncomeResourceCommand>
{
    private readonly IResourceManagementContract _resourceManagementContract;
    
    private readonly IUnitManagementContract _unitManagementContract;
    
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateIncomeResourceCommand> _validator;
    
    private readonly ILogger<UpdateIncomeResourceHandler> _logger;


    public UpdateIncomeResourceHandler(
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract, 
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository, 
        [FromKeyedServices(Modules.IncomeProcessing)]  IUnitOfWork unitOfWork, 
        IValidator<UpdateIncomeResourceCommand> validator, 
        ILogger<UpdateIncomeResourceHandler> logger)
    {
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
        _incomeDocumentRepository = incomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateIncomeResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var incomeDocumentId = IncomeDocumentId.Of(command.IncomeDocumentId);
        
        var incomeDocumentResult = await _incomeDocumentRepository.GetByIdAsync(incomeDocumentId, cancellationToken);
        
        if (incomeDocumentResult.IsFailure) 
            return incomeDocumentResult.Error.ToErrorList();
        
        var incomeDocument = incomeDocumentResult.Value;

        var incomeResourceResult = incomeDocument.GetIncomeResourceById(command.IncomeResourceId);
        
        if (incomeResourceResult.IsFailure) 
            return incomeResourceResult.Error.ToErrorList();
        
        var incomeResource = incomeResourceResult.Value;

        if (incomeResource.ResourceId.Value != command.ResourceId)
        {
            var checkResourceExistsAndNotArchivedByIdResult = await _resourceManagementContract.CheckResourceExistsAndNotArchivedById(command.ResourceId);

            if (checkResourceExistsAndNotArchivedByIdResult.IsFailure)
                return checkResourceExistsAndNotArchivedByIdResult.Error.ToErrorList();
        }

        if (incomeResource.UnitId.Value != command.UnitId)
        {
            var checkUnitExistsAndNotArchivedByIdResult = await _unitManagementContract.CheckUnitExistsAndNotArchivedById(command.UnitId);
        
            if (checkUnitExistsAndNotArchivedByIdResult.IsFailure)
                return checkUnitExistsAndNotArchivedByIdResult.Error.ToErrorList();
        }
        
        incomeResource.UpdateMainInfo(
            ResourceId.Of(command.ResourceId), 
            UnitId.Of(command.UnitId), 
            Stock.Of(command.ResourceStock).Value
            );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("IncomeResource by ${id} has been updated", command.IncomeResourceId);
        
        return command.IncomeDocumentId;
    }
}