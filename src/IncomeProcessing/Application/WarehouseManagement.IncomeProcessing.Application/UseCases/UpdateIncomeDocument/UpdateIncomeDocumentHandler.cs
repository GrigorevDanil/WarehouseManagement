using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.IncomeProcessing.Contracts;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeDocument;

public class UpdateIncomeDocumentHandler : ICommandHandler<Guid, UpdateIncomeDocumentCommand>
{
    private readonly IIncomeProcessingContract _incomeProcessingContract;
    
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateIncomeDocumentCommand> _validator;
    
    private readonly ILogger<UpdateIncomeDocumentHandler> _logger;

    public UpdateIncomeDocumentHandler(
        IIncomeProcessingContract incomeProcessingContract, 
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository,
        [FromKeyedServices(Modules.IncomeProcessing)] IUnitOfWork unitOfWork,
        IValidator<UpdateIncomeDocumentCommand> validator, 
        ILogger<UpdateIncomeDocumentHandler> logger)
    {
        _incomeProcessingContract = incomeProcessingContract;
        _incomeDocumentRepository = incomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateIncomeDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var incomeDocumentId = IncomeDocumentId.Of(command.IncomeDocumentId);
        
        var incomeDocumentResult = await _incomeDocumentRepository.GetByIdAsync(incomeDocumentId, cancellationToken);
        
        if (incomeDocumentResult.IsFailure) 
            return incomeDocumentResult.Error.ToErrorList();
        
        var incomeDocument = incomeDocumentResult.Value;

        if (incomeDocument.NumDocument.Value != command.NumDocument)
        {
            var checkIncomeDocumentNumDocumentNotExistsResult = await _incomeProcessingContract.CheckIncomeDocumentNumDocumentNotExists(command.NumDocument);
        
            if (checkIncomeDocumentNumDocumentNotExistsResult.IsFailure) 
                return checkIncomeDocumentNumDocumentNotExistsResult.Error.ToErrorList();
        }
        
        incomeDocument.UpdateMainInfo(
            NumDocument.Of(command.NumDocument).Value,
            CreatedAt.Of(command.CreatedAt).Value
            );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("IncomeDocument by ${id} has been updated", command.IncomeDocumentId);
        
        return command.IncomeDocumentId;
    }
}