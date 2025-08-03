using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.Core.Abstractions;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Enums;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ClientManagement.Application.UseCases.UpdateClient;

public class UpdateClientHandler : ICommandHandler<Guid, UpdateClientCommand>
{
    private readonly IClientManagementContract _clientManagementContract;
    
    private readonly IRepository<Client,ClientId> _clientRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<UpdateClientCommand> _validator;
    
    private readonly ILogger<UpdateClientHandler> _logger;

    public UpdateClientHandler(
        IClientManagementContract clientManagementContract,
        IRepository<Client, ClientId> clientRepository, 
        [FromKeyedServices(Modules.ClientManagement)] IUnitOfWork unitOfWork,
        IValidator<UpdateClientCommand> validator,
        ILogger<UpdateClientHandler> logger)
    {
        _clientManagementContract = clientManagementContract;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateClientCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var clientId = ClientId.Of(command.ClientId);
        
        var clientResult = await _clientRepository.GetByIdAsync(clientId, cancellationToken);

        if (clientResult.IsFailure)     
            return clientResult.Error.ToErrorList();
        
        var client = clientResult.Value;
        
        if (client.Title.Value != command.Title)
        {
            var checkClientTitleNotExistsResult = await _clientManagementContract.CheckClientTitleNotExists(command.Title);

            if (checkClientTitleNotExistsResult.IsFailure) 
                return checkClientTitleNotExistsResult.Error.ToErrorList();
        }
        
        client.UpdateMainInfo(
            Title.Of(command.Title).Value,
            Address.Of(command.Address).Value
            );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Client by id `${id}` has updated", command.ClientId);
        
        return command.ClientId;
    }
}