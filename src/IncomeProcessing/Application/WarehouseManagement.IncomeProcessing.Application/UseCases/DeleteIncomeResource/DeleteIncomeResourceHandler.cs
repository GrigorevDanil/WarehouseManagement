using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeResource;

public class DeleteIncomeResourceHandler : ICommandHandler<Guid, DeleteIncomeResourceCommand>
{
    private readonly IRepository<IncomeDocument,IncomeDocumentId> _incomeDocumentRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteIncomeResourceCommand> _validator;
    
    private readonly ILogger<DeleteIncomeResourceHandler> _logger;

    public DeleteIncomeResourceHandler(
        IRepository<IncomeDocument, IncomeDocumentId> incomeDocumentRepository,
        [FromKeyedServices(Modules.IncomeProcessing)] IUnitOfWork unitOfWork, 
        IValidator<DeleteIncomeResourceCommand> validator,
        ILogger<DeleteIncomeResourceHandler> logger)
    {
        _incomeDocumentRepository = incomeDocumentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteIncomeResourceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var incomeDocumentId = IncomeDocumentId.Of(command.IncomeDocumentId);
        
        var incomeDocumentResult = await _incomeDocumentRepository.GetByIdAsync(incomeDocumentId, cancellationToken);
        
        if (incomeDocumentResult.IsFailure)
            return incomeDocumentResult.Error.ToErrorList();
        
        var incomeDocument =  incomeDocumentResult.Value;

        var incomeResourceResult = incomeDocument.GetIncomeResourceById(command.IncomeResourceId);
        
        if (incomeResourceResult.IsFailure) 
            return incomeResourceResult.Error.ToErrorList();
        
        var incomeResource = incomeResourceResult.Value;
        
        incomeDocument.DeleteResource(incomeResource);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("IncomeResource by id `${id}` deleted", command.IncomeResourceId);
        
        return command.IncomeResourceId;
    }
}