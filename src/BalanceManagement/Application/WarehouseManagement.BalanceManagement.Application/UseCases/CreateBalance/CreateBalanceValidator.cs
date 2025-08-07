using FluentValidation;
using WarehouseManagement.BalanceManagement.Contracts.Requests;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.CreateBalance;

public class CreateBalanceValidator : AbstractValidator<CreateBalanceCommand>
{
    public CreateBalanceValidator()
    {
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.ResourceQuantity).MustBeValueObject(Stock.Of);
    }
}