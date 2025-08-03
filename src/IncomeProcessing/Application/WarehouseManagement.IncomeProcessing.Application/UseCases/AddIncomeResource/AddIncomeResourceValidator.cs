using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.AddIncomeResource;

public class AddIncomeResourceValidator : AbstractValidator<AddIncomeResourceCommand>
{
    public AddIncomeResourceValidator()
    {
        RuleFor(x => x.IncomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.ResourceStock).MustBeValueObject(Stock.Of);
    }
}