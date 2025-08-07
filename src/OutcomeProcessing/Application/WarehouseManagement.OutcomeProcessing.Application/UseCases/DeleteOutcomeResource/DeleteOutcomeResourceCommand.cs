using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeResource;

public record DeleteOutcomeResourceCommand(Guid OutcomeDocumentId, Guid OutcomeResourceId) : ICommand;