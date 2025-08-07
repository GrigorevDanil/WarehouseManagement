using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeDocument;

public class DeleteOutcomeDocumentValidator : AbstractValidator<DeleteOutcomeDocumentCommand>
{
    public DeleteOutcomeDocumentValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
    }
}