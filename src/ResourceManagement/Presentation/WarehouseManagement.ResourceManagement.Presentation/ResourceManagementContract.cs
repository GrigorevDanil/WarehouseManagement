using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Application.Queries.GetResourcesById;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ResourceManagement.Presentation;

public class ResourceManagementContract : IResourceManagementContract
{
    private readonly IResourceReadDbContext _dbContext;

    private readonly GetResourcesByIdHandler _getResourcesByIdHandler;

    public ResourceManagementContract(
        IResourceReadDbContext dbContext, 
        GetResourcesByIdHandler getResourcesByIdHandler)
    {
        _dbContext = dbContext;
        _getResourcesByIdHandler = getResourcesByIdHandler;
    }

    public async Task<UnitResult<Error>> CheckResourceTitleNotExists(string title)
    {
        var foundedResource = await _dbContext.Resources
            .FirstOrDefaultAsync(s => s.Title == title);

        if (foundedResource is null)
            return Result.Success<Error>();

        return Errors.General.AlreadyExists(nameof(Resource), nameof(Title), title);
    }

    public async Task<UnitResult<Error>> CheckResourceExistsAndNotArchivedById(Guid resourceId)
    {
        var foundedResource = await _dbContext.Resources
            .FirstOrDefaultAsync(s => s.Id == resourceId);

        if (foundedResource is null) return Errors.General.NotFound(resourceId);
        
        if (foundedResource.IsArchived) return Errors.Archive.ArchiveRecord(nameof(Resource), resourceId);
        
        return Result.Success<Error>();
    }

    public async Task<Result<ResourceDto, ErrorList>> GetResourceById(Guid resourceId, CancellationToken cancellationToken = default)
    {
        var query = new GetResourcesByIdQuery(resourceId);
        
        return await _getResourcesByIdHandler.Handle(query, cancellationToken);
    }
}