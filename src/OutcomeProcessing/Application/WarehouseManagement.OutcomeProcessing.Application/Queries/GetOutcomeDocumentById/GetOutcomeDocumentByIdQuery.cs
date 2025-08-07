using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.OutcomeProcessing.Application.Queries.GetOutcomeDocumentById;

public record GetOutcomeDocumentByIdQuery(Guid OutcomeDocumentId) : IQuery;