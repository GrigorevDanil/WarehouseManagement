using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.AddOutcomeResource;

public class AddOutcomeResourceValidator : AbstractValidator<AddOutcomeResourceCommand>
{
    public AddOutcomeResourceValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.ResourceQuantity).MustBeValueObject(Quantity.Of);
    }
}