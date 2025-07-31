using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.RestoreResourceFromArchive;

public class RestoreResourceFromArchiveValidator: AbstractValidator<RestoreResourceFromArchiveCommand>
{
    public RestoreResourceFromArchiveValidator()
    {
        RuleFor(x => x.ResourceId).MustBeValidGuid();
    }
}