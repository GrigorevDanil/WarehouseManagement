using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework;
using WarehouseManagement.Framework.Extensions;
using WarehouseManagement.OutcomeProcessing.Application.Queries.GetOutcomeDocumentById;
using WarehouseManagement.OutcomeProcessing.Application.Queries.GetOutcomeDocumentsWithPagination;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.AddOutcomeResource;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.CreateOutcomeDocumentWithResources;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeDocument;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeResource;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.RevokeDocument;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.SignDocument;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeDocument;
using WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeResource;
using WarehouseManagement.OutcomeProcessing.Contracts.Requests;
using WarehouseManagement.OutcomeProcessing.Contracts.Responses;

namespace WarehouseManagement.OutcomeProcessing.Presentation;

public class OutcomeDocumentController : ApplicationController
{
    /// <summary>
    /// Получить список документов отгрузки с пагинацией
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope<PagedList<OutcomeDocumentResponse>>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<PagedList<OutcomeDocumentResponse>>> GetOutcomeDocumentsWithPagination(
        [FromQuery] GetOutcomeDocumentsWithPaginationRequest request,
        [FromServices] GetOutcomeDocumentsWithPaginationHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetOutcomeDocumentsWithPaginationQuery.Create(request);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Получить документ отгрузки по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор документа отгрузки</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<OutcomeDocumentResponse>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<OutcomeDocumentResponse>> GetOutcomeDocumentById(
        [FromRoute] Guid id,
        [FromServices] GetOutcomeDocumentByIdHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetOutcomeDocumentByIdQuery(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Создание документа отгрузки с ресурсами
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданного документа отгрузки, а также набор Id созданных ресурсов отгрузки</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope<CreateOutcomeDocumentWithResourcesResponse>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<CreateOutcomeDocumentWithResourcesResponse>> CreateOutcomeDocumentWithResources(
        [FromBody] CreateOutcomeDocumentWithResourcesRequest request,
        [FromServices] CreateOutcomeDocumentWithResourcesHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = CreateOutcomeDocumentWithResourcesCommand.Create(request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление документа отгрузки
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор документа отгрузки</param>
    /// <response code="200">Вернется Id обновленного документа отгрузки</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateOutcomeDocument(
        [FromRoute] Guid id,
        [FromForm] UpdateOutcomeDocumentRequest request,
        [FromServices] UpdateOutcomeDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateOutcomeDocumentCommand.Create(id, request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить документ отгрузки по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор документа отгрузки</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteOutcomeDocument(
        [FromRoute] Guid id,
        [FromServices] DeleteOutcomeDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = new DeleteOutcomeDocumentCommand(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Подписание документа отгрузки
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор документа отгрузки</param>
    /// <response code="200">Вернется Id подписанного документа отгрузки</response>
    [HttpPut("{id:guid}/sign")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> SignDocument(
        [FromRoute] Guid id,
        [FromServices] SignDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new SignDocumentCommand(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Отозвать документ отгрузки
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор документа отгрузки</param>
    /// <response code="200">Вернется Id отозванного документа отгрузки</response>
    [HttpPut("{id:guid}/revoke")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> RevokeDocument(
        [FromRoute] Guid id,
        [FromServices] RevokeDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new RevokeDocumentCommand(id);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Добавить ресурс к документу отгрузки
    /// </summary>
    /// <param name="id">Идентификатор документа отгрузки</param>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданного ресурса к документу отгрузки</response>
    [HttpPost("{id:guid}/outcomeResource")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> AddOutcomeResource(
        [FromRoute] Guid id,
        [FromForm] AddOutcomeResourceRequest request,
        [FromServices] AddOutcomeResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = AddOutcomeResourceCommand.Create(id,request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление ресурса в документе отгрузки
    /// </summary>
    /// <returns></returns>
    /// <param name="outcomeDocumentId">Идентификатор документа отгрузки</param>
    /// <param name="outcomeResourceId">Идентификатор ресурса в документе отгрузки</param>
    /// <response code="200">Вернется Id обновленного ресурса из документа отгрузки</response>
    [HttpPut("{outcomeDocumentId:guid}/outcomeResource/{outcomeResourceId:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateOutcomeResource(
        [FromRoute] Guid outcomeDocumentId,
        [FromRoute] Guid outcomeResourceId,
        [FromForm] UpdateOutcomeResourceRequest request,
        [FromServices] UpdateOutcomeResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateOutcomeResourceCommand.Create(outcomeDocumentId, outcomeResourceId, request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить ресурс из документа отгрузки
    /// </summary>
    /// <returns></returns>
    /// <param name="outcomeDocumentId">Идентификатор документа отгрузки</param>
    /// <param name="outcomeResourceId">Идентификатор ресурса в документе отгрузки</param>
    /// <response code="200">Вернется Id удаленного ресурса из документа отгрузки</response>
    [HttpDelete("{outcomeDocumentId:guid}/outcomeResource/{outcomeResourceId:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteOutcomeResource(
        [FromRoute] Guid outcomeDocumentId,
        [FromRoute] Guid outcomeResourceId,
        [FromServices] DeleteOutcomeResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new DeleteOutcomeResourceCommand(outcomeDocumentId, outcomeResourceId);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
}