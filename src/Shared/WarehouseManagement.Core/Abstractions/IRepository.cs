using System.Security.Principal;
using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.Core.Abstractions;

public interface IRepository<TEntity, TId> where TEntity : class
{
    Task<TId> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    TId Delete(TEntity entity);
    
    Task<Result<TEntity, Error>> GetByIdAsync(TId entityId, CancellationToken cancellationToken = default);
}