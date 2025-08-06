using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.ReplenishResourceStockBalance;

public class ReplenishResourceStockBalanceValidator : AbstractValidator<ReplenishResourceStockBalanceCommand>
{
    public ReplenishResourceStockBalanceValidator()
    {
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.AddedResourceStock).MustBeValueObject(Stock.Of);
    }
}