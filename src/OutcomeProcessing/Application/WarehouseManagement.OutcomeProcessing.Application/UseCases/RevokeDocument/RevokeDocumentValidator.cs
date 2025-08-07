using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.RevokeDocument;

public class RevokeDocumentValidator : AbstractValidator<RevokeDocumentCommand>
{
    public RevokeDocumentValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
    }
}