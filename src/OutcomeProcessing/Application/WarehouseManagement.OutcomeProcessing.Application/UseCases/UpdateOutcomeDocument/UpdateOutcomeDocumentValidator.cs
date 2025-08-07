using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeDocument;

public class UpdateOutcomeDocumentValidator : AbstractValidator<UpdateOutcomeDocumentCommand>
{
    public UpdateOutcomeDocumentValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.NumDocument).MustBeValueObject(NumDocument.Of);
        RuleFor(x => x.ClientId).MustBeValidGuid();
        RuleFor(x => x.CreatedAt).MustBeValueObject(CreatedAt.Of);
    }
}