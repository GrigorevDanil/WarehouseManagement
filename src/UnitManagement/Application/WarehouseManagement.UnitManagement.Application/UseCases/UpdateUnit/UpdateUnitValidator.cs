using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.UnitManagement.Application.UseCases.UpdateUnit;

public class UpdateUnitValidator : AbstractValidator<UpdateUnitCommand>
{
    public UpdateUnitValidator()
    {
        RuleFor(x => x.UnitId).MustBeValidGuid();
        RuleFor(x => x.Title).MustBeValueObject(Title.Of);
    }
}