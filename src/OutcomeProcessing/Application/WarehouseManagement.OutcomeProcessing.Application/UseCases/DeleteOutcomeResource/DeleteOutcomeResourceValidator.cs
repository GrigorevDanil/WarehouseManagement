using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.DeleteOutcomeResource;

public class DeleteOutcomeResourceValidator : AbstractValidator<DeleteOutcomeResourceCommand>
{
    public DeleteOutcomeResourceValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.OutcomeResourceId).MustBeValidGuid();
    }
}