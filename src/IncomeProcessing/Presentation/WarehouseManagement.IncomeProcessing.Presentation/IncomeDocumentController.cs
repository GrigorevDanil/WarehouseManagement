using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework;
using WarehouseManagement.Framework.Extensions;
using WarehouseManagement.IncomeProcessing.Application.Queries.GetIncomeDocumentById;
using WarehouseManagement.IncomeProcessing.Application.Queries.GetIncomeDocumentsWithPagination;
using WarehouseManagement.IncomeProcessing.Application.UseCases.AddIncomeResource;
using WarehouseManagement.IncomeProcessing.Application.UseCases.CreateIncomeDocument;
using WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeDocument;
using WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeResource;
using WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeDocument;
using WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeResource;
using WarehouseManagement.IncomeProcessing.Contracts.Requests;
using WarehouseManagement.IncomeProcessing.Contracts.Responses;

namespace WarehouseManagement.IncomeProcessing.Presentation;

public class IncomeDocumentController : ApplicationController
{
    /// <summary>
    /// Получить список документов поступления с пагинацией
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope<PagedList<IncomeDocumentResponse>>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<PagedList<IncomeDocumentResponse>>> GetIncomeDocumentsWithPagination(
        [FromQuery] GetIncomeDocumentsWithPaginationRequest request,
        [FromServices] GetIncomeDocumentsWithPaginationHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetIncomeDocumentsWithPaginationQuery.Create(request);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Получить документ поступления по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор документа поступления</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<IncomeDocumentResponse>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<IncomeDocumentResponse>> GetIncomeDocumentById(
        [FromRoute] Guid id,
        [FromQuery] GetIncomeDocumentByIdRequest request,
        [FromServices] GetIncomeDocumentByIdHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetIncomeDocumentByIdQuery.Create(id, request);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Создание документа поступления
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданного документа поступления</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> CreateIncomeDocument(
        [FromForm] CreateIncomeDocumentRequest request,
        [FromServices] CreateIncomeDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = CreateIncomeDocumentCommand.Create(request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление документа поступления
    /// </summary>
    /// <returns></returns>
    /// <param name="id">Идентификатор документа поступления</param>
    /// <response code="200">Вернется Id обновленного документа поступления</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateIncomeDocument(
        [FromRoute] Guid id,
        [FromForm] UpdateIncomeDocumentRequest request,
        [FromServices] UpdateIncomeDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateIncomeDocumentCommand.Create(id,  request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить документ поступления по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор документа поступления</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteIncomeDocument(
        [FromRoute] Guid id,
        [FromServices] DeleteIncomeDocumentHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var query = DeleteIncomeDocumentCommand.Create(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Добавить ресурс к документу поступления
    /// </summary>
    /// <param name="id">Идентификатор документа поступления</param>
    /// <returns></returns>
    /// <response code="200">Вернется Id созданного ресурса к документу поступления</response>
    [HttpPost("{id:guid}/incomeResource")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> AddIncomeResource(
        [FromRoute] Guid id,
        [FromForm] AddIncomeResourceRequest request,
        [FromServices] AddIncomeResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = AddIncomeResourceCommand.Create(id,request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Обновление ресурса в документе поступления
    /// </summary>
    /// <returns></returns>
    /// <param name="incomeDocumentId">Идентификатор документа поступления</param>
    /// <param name="incomeResourceId">Идентификатор ресурса в документе поступления</param>
    /// <response code="200">Вернется Id обновленного ресурса из документа поступления</response>
    [HttpPut("{incomeDocumentId:guid}/incomeResource/{incomeResourceId:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> UpdateIncomeResource(
        [FromRoute] Guid incomeDocumentId,
        [FromRoute] Guid incomeResourceId,
        [FromForm] UpdateIncomeResourceRequest request,
        [FromServices] UpdateIncomeResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = UpdateIncomeResourceCommand.Create(incomeDocumentId, incomeResourceId, request);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
    
    /// <summary>
    /// Удалить ресурс из документа поступления
    /// </summary>
    /// <returns></returns>
    /// <param name="incomeDocumentId">Идентификатор документа поступления</param>
    /// <param name="incomeResourceId">Идентификатор ресурса в документе поступления</param>
    /// <response code="200">Вернется Id удаленного ресурса из документа поступления</response>
    [HttpDelete("{incomeDocumentId:guid}/incomeResource/{incomeResourceId:guid}")]
    [ProducesResponseType(typeof(Envelope<Guid>), StatusCodes.Status200OK), ]
    public async Task<ActionResult<Guid>> DeleteIncomeResource(
        [FromRoute] Guid incomeDocumentId,
        [FromRoute] Guid incomeResourceId,
        [FromServices] DeleteIncomeResourceHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = DeleteIncomeResourceCommand.Create(incomeDocumentId, incomeResourceId);
        var result = await handler.Handle(command, cancellationToken);
        return result.ToResponse();
    }
}