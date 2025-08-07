using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.OutcomeProcessing.Domain.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.CreateOutcomeDocumentWithResources;

public class CreateOutcomeDocumentWithResourcesValidator : AbstractValidator<CreateOutcomeDocumentWithResourcesCommand>
{
    public CreateOutcomeDocumentWithResourcesValidator()
    {
        RuleFor(x => x.NumDocument).MustBeValueObject(NumDocument.Of);
        RuleFor(x => x.ClientId).MustBeValidGuid();
        RuleFor(x => x.CreatedAt).MustBeValueObject(CreatedAt.Of);
        
        RuleFor(x => x.Items).MustNotBeEmptyArray();
        
        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ResourceId).MustBeValidGuid();
                item.RuleFor(x => x.UnitId).MustBeValidGuid();
                item.RuleFor(x => x.ResourceQuantity).MustBeValueObject(Quantity.Of);
            });
    }
}