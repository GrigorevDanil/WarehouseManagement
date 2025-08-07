using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeDocument;

public record DeleteOutcomeDocumentCommand(Guid OutcomeDocumentId) : ICommand;