using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ClientManagement.Application.Interfaces;
using WarehouseManagement.ClientManagement.Contracts;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ClientManagement.Presentation;

public class ClientManagementContract : IClientManagementContract
{
    private readonly IClientReadDbContext _dbContext;

    public ClientManagementContract(IClientReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnitResult<Error>> CheckClientTitleNotExists(string title)
    {
        var foundedClients = await _dbContext.Clients
            .FirstOrDefaultAsync(s => s.Title == title);

        if (foundedClients is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(Client), nameof(Title), title);
    }
    
}