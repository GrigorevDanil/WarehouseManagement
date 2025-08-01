using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.UnitManagement.Application.UseCases.MoveUnitToArchive;

public class MoveUnitToArchiveValidator : AbstractValidator<MoveUnitToArchiveCommand>
{
    public MoveUnitToArchiveValidator()
    {
        RuleFor(x => x.UnitId).MustBeValidGuid();
    }
}