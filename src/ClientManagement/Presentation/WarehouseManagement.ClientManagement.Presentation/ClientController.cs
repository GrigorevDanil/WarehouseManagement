using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.ClientManagement.Application.Queries.GetClientById;
using WarehouseManagement.ClientManagement.Application.Queries.GetClientsWithPagination;
using WarehouseManagement.ClientManagement.Application.UseCases.CreateClient;
using WarehouseManagement.ClientManagement.Application.UseCases.DeleteClient;
using WarehouseManagement.ClientManagement.Application.UseCases.MoveClientToArchive;
using WarehouseManagement.ClientManagement.Application.UseCases.RestoreClientFromArchive;
using WarehouseManagement.ClientManagement.Application.UseCases.UpdateClient;
using WarehouseManagement.ClientManagement.Contracts.Dtos;
using WarehouseManagement.ClientManagement.Contracts.Requests;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework;
using WarehouseManagement.Framework.Extensions;

namespace WarehouseManagement.ClientManagement.Presentation;

public class ClientController : ApplicationController
{
    /// <summary>
    /// Получить список клиентов с пагинацией
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope<PagedList<ClientDto>>), StatusCodes.Status200OK), ]
    public async Task<ActionResult> GetClientsWithPagination(
        [FromQuery] GetClientsWithPaginationRequest request,
        [FromServices] GetClientsWithPaginationHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetClientsWithPaginationQuery.Create(request);
        var response = await handler.Handle(query, cancellationToken);
        return Ok(response);
    }
    
    /// <summary>
    /// Получить клиента по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<ClientDto>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<ClientDto>> GetClientById(
        [FromRoute] Guid id,
        [FromServices] GetClientByIdHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetClientByIdQuery.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Создание клиента
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданного клиента</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> CreateClient(
        [FromForm] CreateClientRequest request,
        [FromServices] CreateClientHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = CreateClientCommand.Create(request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление клиента
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор клиента</param>
    /// <response code="200">Вернется Id обновленного клиента</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateClient(
        [FromRoute] Guid id,
        [FromForm] UpdateClientRequest request,
        [FromServices] UpdateClientHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateClientCommand.Create(id,  request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить клиента по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteClient(
        [FromRoute] Guid id,
        [FromServices] DeleteClientHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = DeleteClientCommand.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Перенос клиента в архив
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор клиента</param>
    /// <response code="200">Вернется Id клиента помещенного в архив</response>
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> MoveClientToArchive(
        [FromRoute] Guid id,
        [FromServices] MoveClientToArchiveHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = MoveClientToArchiveCommand.Create(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Вернуть клиента из архива
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор клиента</param>
    /// <response code="200">Вернется Id клиента возвращенного из архива</response>
    [HttpPost("{id:guid}/archive/restore")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> RestoreClientFromArchive(
        [FromRoute] Guid id,
        [FromServices] RestoreClientFromArchiveHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = RestoreClientFromArchiveCommand.Create(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
}