using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.Core.Abstractions.Messages;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TResponse>
{
    public Task<TResponse> Handle(CancellationToken cancellationToken = default);
}

public interface IQueryHandlerWithResult<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Handle(TQuery query, CancellationToken cancellationToken = default);
}



