using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.UpdateOutcomeResource;

public class UpdateOutcomeResourceValidator : AbstractValidator<UpdateOutcomeResourceCommand>
{
    public UpdateOutcomeResourceValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.OutcomeResourceId).MustBeValidGuid();
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.ResourceQuantity).MustBeValueObject(Quantity.Of);
    }
}