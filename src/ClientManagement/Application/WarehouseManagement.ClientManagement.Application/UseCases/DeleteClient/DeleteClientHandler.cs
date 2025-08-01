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

namespace WarehouseManagement.ClientManagement.Application.UseCases.DeleteClient;

public class DeleteClientHandler : ICommandHandler<Guid, DeleteClientCommand>
{
    private readonly IRepository<Client,ClientId> _clientRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<DeleteClientCommand> _validator;
    
    private readonly ILogger<DeleteClientHandler> _logger;

    public DeleteClientHandler(
        IRepository<Client, ClientId> clientRepository, 
        [FromKeyedServices(Modules.ClientManagement)] IUnitOfWork unitOfWork, 
        IValidator<DeleteClientCommand> validator,
        ILogger<DeleteClientHandler> logger)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteClientCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var clientId = ClientId.Of(command.ClientId);
        
        var clientResult = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
        
        if (clientResult.IsFailure)
            return clientResult.Error.ToErrorList();
        
        var client =  clientResult.Value;
        
        _clientRepository.Delete(client);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Client by id `${id}` deleted", command.ClientId);
        
        return command.ClientId;
    }
}