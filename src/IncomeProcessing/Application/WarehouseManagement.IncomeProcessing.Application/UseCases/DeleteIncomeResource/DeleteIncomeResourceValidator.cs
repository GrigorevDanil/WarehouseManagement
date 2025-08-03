using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeResource;

public class DeleteIncomeResourceValidator : AbstractValidator<DeleteIncomeResourceCommand>
{
    public DeleteIncomeResourceValidator()
    {
        RuleFor(x => x.IncomeResourceId).MustBeValidGuid();
    }
}