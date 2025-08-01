using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.ClientManagement.Application.UseCases.DeleteClient;

public class DeleteClientValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientValidator()
    {
        RuleFor(x => x.ClientId).MustBeValidGuid();
    }
}