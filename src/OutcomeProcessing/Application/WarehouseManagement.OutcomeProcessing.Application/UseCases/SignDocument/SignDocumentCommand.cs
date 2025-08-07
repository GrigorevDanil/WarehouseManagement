using WarehouseManagement.Core.Abstractions.Messages;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.SignDocument;

public record SignDocumentCommand(Guid OutcomeDocumentId) : ICommand;