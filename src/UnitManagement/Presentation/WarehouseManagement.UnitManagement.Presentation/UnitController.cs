using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework;
using WarehouseManagement.Framework.Extensions;
using WarehouseManagement.UnitManagement.Application.Queries.GetUnitById;
using WarehouseManagement.UnitManagement.Application.Queries.GetUnitsWithPaginations;
using WarehouseManagement.UnitManagement.Application.UseCases.CreateUnit;
using WarehouseManagement.UnitManagement.Application.UseCases.DeleteUnit;
using WarehouseManagement.UnitManagement.Application.UseCases.MoveUnitToArchive;
using WarehouseManagement.UnitManagement.Application.UseCases.RestoreUnitFromArchive;
using WarehouseManagement.UnitManagement.Application.UseCases.UpdateUnit;
using WarehouseManagement.UnitManagement.Contracts.Dtos;
using WarehouseManagement.UnitManagement.Contracts.Requests;

namespace WarehouseManagement.UnitManagement.Presentation;

public class UnitController : ApplicationController
{
    /// <summary>
    /// Получить список единиц измерения с пагинацией
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope<PagedList<UnitDto>>), StatusCodes.Status200OK), ]
    public async Task<ActionResult> GetUnitsWithPagination(
        [FromQuery] GetUnitsWithPaginationsRequest request,
        [FromServices] GetUnitsWithPaginationsHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetUnitsWithPaginationsQuery.Create(request);
        var response = await handler.Handle(query, cancellationToken);
        return Ok(response);
    }
    
    /// <summary>
    /// Получить единицу измерения по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<UnitDto>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<UnitDto>> GetUnitById(
        [FromRoute] Guid id,
        [FromServices] GetUnitByIdHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetUnitByIdQuery.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Создание единицы измерения
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданной единицы измерения</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> CreateUnit(
        [FromForm] CreateUnitRequest request,
        [FromServices] CreateUnitHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = CreateUnitCommand.Create(request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление единицы измерения
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор единицы измерения</param>
    /// <response code="200">Вернется Id обновленной единицы измерения</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateUnit(
        [FromRoute] Guid id,
        [FromForm] UpdateUnitRequest request,
        [FromServices] UpdateUnitHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateUnitCommand.Create(id,  request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить единицу измерения по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteUnit(
        [FromRoute] Guid id,
        [FromServices] DeleteUnitHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = DeleteUnitCommand.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Перенос единицы измерения в архив
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор единицы измерения</param>
    /// <response code="200">Вернется Id единицы измерения помещенной в архив</response>
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> MoveUnitToArchive(
        [FromRoute] Guid id,
        [FromServices] MoveUnitToArchiveHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = MoveUnitToArchiveCommand.Create(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Вернуть единицу измерения из архива
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор единицы измерения</param>
    /// <response code="200">Вернется Id единицы измерения возвращенной из архива</response>
    [HttpPost("{id:guid}/archive/restore")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> RestoreUnitFromArchive(
        [FromRoute] Guid id,
        [FromServices] RestoreUnitFromArchiveHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = RestoreUnitFromArchiveCommand.Create(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
}