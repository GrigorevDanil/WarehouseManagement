using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.ClientManagement.Application.UseCases.MoveClientToArchive;

public class MoveClientToArchiveValidator : AbstractValidator<MoveClientToArchiveCommand>
{
    public MoveClientToArchiveValidator()
    {
        RuleFor(x => x.ClientId).MustBeValidGuid();
    }
}