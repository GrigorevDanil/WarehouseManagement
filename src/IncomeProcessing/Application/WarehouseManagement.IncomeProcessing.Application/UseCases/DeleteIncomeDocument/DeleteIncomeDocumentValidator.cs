using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.DeleteIncomeDocument;

public class DeleteIncomeDocumentValidator : AbstractValidator<DeleteIncomeDocumentCommand>
{
    public DeleteIncomeDocumentValidator()
    {
        RuleFor(x => x.IncomeDocumentId).MustBeValidGuid();
    }
}