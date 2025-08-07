using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework;
using WarehouseManagement.Framework.Extensions;
using WarehouseManagement.ResourceManagement.Application.Queries.GetResourceById;
using WarehouseManagement.ResourceManagement.Application.Queries.GetResourcesWithPagination;
using WarehouseManagement.ResourceManagement.Application.UseCases.CreateResource;
using WarehouseManagement.ResourceManagement.Application.UseCases.DeleteResource;
using WarehouseManagement.ResourceManagement.Application.UseCases.MoveResourceToArchive;
using WarehouseManagement.ResourceManagement.Application.UseCases.RestoreResourceFromArchive;
using WarehouseManagement.ResourceManagement.Application.UseCases.UpdateResource;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;
using WarehouseManagement.ResourceManagement.Contracts.Requests;

namespace WarehouseManagement.ResourceManagement.Presentation;

public class ResourceController : ApplicationController
{
    /// <summary>
    /// Получить список ресурсов с пагинацией
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope<PagedList<ResourceDto>>), StatusCodes.Status200OK), ]
    public async Task<ActionResult> GetResourcesWithPagination(
        [FromQuery] GetResourcesWithPaginationRequest request,
        [FromServices] GetResourcesWithPaginationHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetResourcesWithPaginationQuery.Create(request);
        var response = await handler.Handle(query, cancellationToken);
        return Ok(response);
    }
    
    /// <summary>
    /// Получить ресурс по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<ResourceDto>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<ResourceDto>> GetResourceById(
        [FromRoute] Guid id,
        [FromServices] GetResourceByIdHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetResourceByIdQuery.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Создание ресурса
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданного ресурса</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> CreateResource(
        [FromForm] CreateResourceRequest request,
        [FromServices] CreateResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = CreateResourceCommand.Create(request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление ресурса
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор ресурса</param>
    /// <response code="200">Вернется Id обновленного ресурса</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateResource(
        [FromRoute] Guid id,
        [FromForm] UpdateResourceRequest request,
        [FromServices] UpdateResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateResourceCommand.Create(id,  request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить ресурс по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteResource(
        [FromRoute] Guid id,
        [FromServices] DeleteResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = DeleteResourceCommand.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Перенос ресурса в архив
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор ресурса</param>
    /// <response code="200">Вернется Id ресурса помещенного в архив</response>
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> MoveResourceToArchive(
        [FromRoute] Guid id,
        [FromServices] MoveResourceToArchiveHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = MoveResourceToArchiveCommand.Create(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Вернуть ресурс из архива
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор ресурса</param>
    /// <response code="200">Вернется Id ресурса возвращенного из архива</response>
    [HttpPost("{id:guid}/archive/restore")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> RestoreResourceFromArchive(
        [FromRoute] Guid id,
        [FromServices] RestoreResourceFromArchiveHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = RestoreResourceFromArchiveCommand.Create(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
}