using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.UnitManagement.Application.UseCases.DeleteUnit;

public class DeleteUnitValidator : AbstractValidator<DeleteUnitCommand>
{
    public DeleteUnitValidator()
    {
        RuleFor(x => x.UnitId).MustBeValidGuid();
    }
}