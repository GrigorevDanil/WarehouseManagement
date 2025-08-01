using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.UnitManagement.Application.UseCases.RestoreUnitFromArchive;

public class RestoreUnitFromArchiveValidator : AbstractValidator<RestoreUnitFromArchiveCommand>
{
    public RestoreUnitFromArchiveValidator()
    {
        RuleFor(x => x.UnitId).MustBeValidGuid();
    }
}