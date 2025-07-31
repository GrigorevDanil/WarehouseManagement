using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ResourceManagement.Application.UseCases.UpdateResource;

public class UpdateResourceValidator : AbstractValidator<UpdateResourceCommand>
{
    public UpdateResourceValidator()
    {
        RuleFor(x => x.Title).MustBeValueObject(Title.Of);
    }
}