using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.BalanceManagement.Application.Queries.GetBalancesWithPagination;
using WarehouseManagement.BalanceManagement.Application.UseCases.CreateBalance;
using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.BalanceManagement.Contracts.Responses;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework;
using WarehouseManagement.Framework.Extensions;

namespace WarehouseManagement.BalanceManagement.Presentation;

public class BalanceController : ApplicationController
{
    /// <summary>
    /// Получить список балансов с пагинацией
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope<PagedList<BalanceResponse>>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<PagedList<BalanceResponse>>> GetBalancesWithPagination(
        [FromQuery] GetBalancesWithPaginationRequest request,
        [FromServices] GetBalancesWithPaginationHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetBalancesWithPaginationQuery.Create(request);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
}