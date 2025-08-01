using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.ClientManagement.Application.UseCases.UpdateClient;

public class UpdateClientValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientValidator()
    {
        RuleFor(x => x.ClientId).MustBeValidGuid();
        RuleFor(x => x.Title).MustBeValueObject(Title.Of);
        RuleFor(x => x.Address).MustBeValueObject(Address.Of);
    }
}