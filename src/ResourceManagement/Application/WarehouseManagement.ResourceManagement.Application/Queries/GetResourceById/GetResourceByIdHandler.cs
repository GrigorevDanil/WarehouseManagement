using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.ResourceManagement.Application.Queries.GetResourceById;

public class GetResourceByIdHandler : IQueryHandlerWithResult<ResourceDto, GetResourceByIdQuery>
{
    private readonly IResourceReadDbContext _dbContext;
    

    public GetResourceByIdHandler(IResourceReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<ResourceDto, ErrorList>> Handle(GetResourceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var resource = await _dbContext.Resources.FirstOrDefaultAsync(x => x.Id == query.ResourceId, cancellationToken);
        
        if (resource == null) 
            return Errors.General.NotFound(query.ResourceId).ToErrorList();

        return resource;
    }
}