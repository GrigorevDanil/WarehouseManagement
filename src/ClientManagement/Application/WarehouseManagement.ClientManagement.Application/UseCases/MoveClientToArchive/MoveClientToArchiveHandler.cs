using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ClientManagement.Application.UseCases.MoveClientToArchive;

public class MoveClientToArchiveHandler : ICommandHandler<Guid, MoveClientToArchiveCommand>
{
    private readonly IRepository<Client,ClientId> _clientRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<MoveClientToArchiveCommand> _validator;
    
    private readonly ILogger<MoveClientToArchiveHandler> _logger;

    public MoveClientToArchiveHandler(
        IRepository<Client, ClientId> clientRepository,
        [FromKeyedServices(Modules.ClientManagement)] IUnitOfWork unitOfWork,
        IValidator<MoveClientToArchiveCommand> validator,
        ILogger<MoveClientToArchiveHandler> logger)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(MoveClientToArchiveCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();

        var clientId = ClientId.Of(command.ClientId);
        
        var clientResult = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error.ToErrorList();
        
        var client = clientResult.Value;
        
        client.MoveToArchive();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Client by id `${id}` moved to archive", command.ClientId);
        
        return command.ClientId;
    }
}