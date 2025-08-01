using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.ClientManagement.Application.UseCases.RestoreClientFromArchive;

public class RestoreClientFromArchiveValidator : AbstractValidator<RestoreClientFromArchiveCommand>
{
    public RestoreClientFromArchiveValidator()
    {
        RuleFor(x => x.ClientId).MustBeValidGuid();
    }
}