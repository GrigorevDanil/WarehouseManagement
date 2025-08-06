using CSharpFunctionalExtensions;
using WarehouseManagement.BalanceManagement.Application.Interfaces;
using WarehouseManagement.BalanceManagement.Contracts.Responses;
using WarehouseManagement.Core.Abstractions.Messages;
using WarehouseManagement.Core.Extensions;
using WarehouseManagement.Core.Models;
using WarehouseManagement.ResourceManagement.Contracts;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.UnitManagement.Contracts;

namespace WarehouseManagement.BalanceManagement.Application.Queries.GetBalancesWithPagination;

public class GetBalancesWithPaginationHandler : IQueryHandlerWithResult<PagedList<BalanceResponse>, GetBalancesWithPaginationQuery>
{
    private readonly IBalanceReadDbContext _dbContext;
    
    private readonly IResourceManagementContract  _resourceManagementContract;
    
    private readonly IUnitManagementContract  _unitManagementContract;

    public GetBalancesWithPaginationHandler(
        IBalanceReadDbContext dbContext, 
        IResourceManagementContract resourceManagementContract,
        IUnitManagementContract unitManagementContract)
    {
        _dbContext = dbContext;
        _resourceManagementContract = resourceManagementContract;
        _unitManagementContract = unitManagementContract;
    }


    public async Task<Result<PagedList<BalanceResponse>, ErrorList>> Handle(GetBalancesWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var balanceQuery = _dbContext.Balances;

        balanceQuery = balanceQuery
            .WhereIf(query.ResourceIds != null!,
                b => query.ResourceIds!.Contains(b.ResourceId))
            .WhereIf(query.UnitIds != null!,
                b => query.UnitIds!.Contains(b.UnitId));
        
        var balanceResponses = new List<BalanceResponse>();

        foreach (var balanceDto in balanceQuery)
        {
            var resourceResult = await _resourceManagementContract.GetResourceById(balanceDto.ResourceId, cancellationToken);
                
            if (resourceResult.IsFailure) 
                return resourceResult.Error;
                
            var resourceDto = resourceResult.Value;
                
            var unitResult = await _unitManagementContract.GetUnitById(balanceDto.UnitId, cancellationToken);
                
            if (unitResult.IsFailure) 
                return unitResult.Error;
                
            var unitDto = unitResult.Value;
                
            balanceResponses.Add(
                new BalanceResponse(
                    Id: balanceDto.Id,
                    Resource: new BalanceResourceResponse(
                        Id: resourceDto.Id,
                        Title: resourceDto.Title
                    ),
                    Unit: new BalanceUnitResponse(
                        Id: unitDto.Id,
                        Title: unitDto.Title
                    ),
                    ResourceStock: balanceDto.ResourceStock
                ));
        }
        
        balanceResponses = new List<BalanceResponse>(query.SortDirection?.ToLower() == "desc"
            ? balanceResponses.OrderByDescending(res => res.Resource.Title)
            : balanceResponses.OrderBy(res => res.Resource.Title));

        return balanceResponses.ToPagedList(query.Page, query.PageSize);
    }
}