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

namespace WarehouseManagement.ClientManagement.Application.UseCases.CreateClient;

public class CreateClientHandler : ICommandHandler<Guid, CreateClientCommand>
{
    private readonly IClientManagementContract _clientManagementContract;
    
    private readonly IRepository<Client,ClientId> _clientRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IValidator<CreateClientCommand> _validator;
    
    private readonly ILogger<CreateClientHandler> _logger;

    public CreateClientHandler(
        IClientManagementContract clientManagementContract,
        IRepository<Client, ClientId> clientRepository, 
        [FromKeyedServices(Modules.ClientManagement)] IUnitOfWork unitOfWork, 
        IValidator<CreateClientCommand> validator,
        ILogger<CreateClientHandler> logger)
    {
        _clientManagementContract = clientManagementContract;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateClientCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
            return validationResult.ToErrorList();
        
        var checkClientTitleNotExistsResult = await _clientManagementContract.CheckClientTitleNotExists(command.Title);

        if (checkClientTitleNotExistsResult.IsFailure) 
            return checkClientTitleNotExistsResult.Error.ToErrorList();

        var client = new Client(
            Title.Of(command.Title).Value,
            Address.Of(command.Address).Value
            );
        
        var clientId = await _clientRepository.AddAsync(client, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Client named ${title} has been created", command.Title);
        
        return clientId.Value;
    }
}