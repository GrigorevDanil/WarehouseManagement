using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.BalanceManagement.Application.UseCases.ReduceResourceStockBalance;

public class ReduceResourceStockBalanceValidator : AbstractValidator<ReduceResourceStockBalanceCommand>
{
    public ReduceResourceStockBalanceValidator()
    {
        RuleFor(x => x.ResourceId).MustBeValidGuid();
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.SubtractedResourceStock).MustBeValueObject(Stock.Of);
    }
}