using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.MoveResourceToArchive;

public class MoveResourceToArchiveValidator : AbstractValidator<MoveResourceToArchiveCommand>
{
    public MoveResourceToArchiveValidator()
    {
        RuleFor(x => x.ResourceId).MustBeValidGuid();
    }
}