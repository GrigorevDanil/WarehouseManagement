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
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.CreateIncomeDocument;

public class CreateIncomeDocumentHandler : ICommandHandler<Guid, CreateIncomeDocumentCommand>
{
    private readonly IIncomeProcessingContract _incomeProcessingContract;
    
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<CreateIncomeDocumentCommand> _validator;
    
    private readonly ILogger<CreateIncomeDocumentHandler> _logger;

    public CreateIncomeDocumentHandler(
        IIncomeProcessingContract incomeProcessingContract,
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository,
        [FromKeyedServices(Modules.IncomeProcessing)] IUnitOfWork unitOfWork, 
        IValidator<CreateIncomeDocumentCommand> validator,
        ILogger<CreateIncomeDocumentHandler> logger)
    {
        _incomeProcessingContract = incomeProcessingContract;
        _incomeDocumentRepository = incomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateIncomeDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkIncomeDocumentNumDocumentNotExistsResult = await _incomeProcessingContract.CheckIncomeDocumentNumDocumentNotExists(command.NumDocument);
        
        if (checkIncomeDocumentNumDocumentNotExistsResult.IsFailure) 
            return checkIncomeDocumentNumDocumentNotExistsResult.Error.ToErrorList();
        
        var incomeDocument = new IncomeDocument(
            NumDocument.Of(command.NumDocument).Value,
            CreatedAt.Of(command.CreatedAt).Value
        );
        
        var incomeDocumentId = await _incomeDocumentRepository.AddAsync(incomeDocument, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Income document by ${numDocument} has been created", command.NumDocument);
        
        return incomeDocumentId.Value;
    }
}