using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeResource;

public class UpdateIncomeResourceValidator :  AbstractValidator<UpdateIncomeResourceCommand>
{
    public UpdateIncomeResourceValidator()
    {
        RuleFor(x => x.IncomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.IncomeResourceId).MustBeValidGuid();
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.ResourceStock).MustBeValueObject(Stock.Of);
    }
}