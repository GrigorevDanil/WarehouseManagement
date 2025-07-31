using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ResourceManagement.Presentation;

public class ResourceManagementContract : IResourceManagementContract
{
    private readonly IResourceReadDbContext _dbContext;

    public ResourceManagementContract(IResourceReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnitResult<Error>> CheckResourceTitleNotExists(string title)
    {
        var foundedSpecies = await _dbContext.Resources
            .FirstOrDefaultAsync(s => s.Title == title);

        if (foundedSpecies is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(Resource), nameof(Title), title);
    }
}