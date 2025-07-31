using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.DeleteResource;

public class DeleteResourceValidator : AbstractValidator<DeleteResourceCommand>
{
    public DeleteResourceValidator()
    {
        RuleFor(x => x.ResourceId).MustBeValidGuid();
    }
}