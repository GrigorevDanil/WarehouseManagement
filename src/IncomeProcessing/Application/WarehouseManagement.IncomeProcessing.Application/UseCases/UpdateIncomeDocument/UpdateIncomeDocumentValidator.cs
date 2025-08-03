using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.UpdateIncomeDocument;

public class UpdateIncomeDocumentValidator : AbstractValidator<UpdateIncomeDocumentCommand>
{
    public UpdateIncomeDocumentValidator()
    {
        RuleFor(x => x.IncomeDocumentId).MustBeValidGuid();
        RuleFor(x => x.NumDocument).MustBeValueObject(NumDocument.Of);
        RuleFor(x => x.CreatedAt).MustBeValueObject(CreatedAt.Of);
    }
}