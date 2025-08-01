using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ClientManagement.Application.UseCases.CreateClient;

public class CreateClientValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientValidator()
    {
        RuleFor(x => x.Title).MustBeValueObject(Title.Of);
        RuleFor(x => x.Address).MustBeValueObject(Address.Of);
    }
}