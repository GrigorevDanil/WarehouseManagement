using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.RevokeDocument;

public record RevokeDocumentCommand(Guid OutcomeDocumentId) : ICommand;