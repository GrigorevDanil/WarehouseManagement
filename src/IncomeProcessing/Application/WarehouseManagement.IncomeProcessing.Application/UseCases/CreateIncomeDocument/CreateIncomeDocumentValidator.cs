using FluentValidation;
using WarehouseManagement.Core.Validation;
using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.IncomeProcessing.Application.UseCases.CreateIncomeDocument;

public class CreateIncomeDocumentValidator : AbstractValidator<CreateIncomeDocumentCommand>
{
    public CreateIncomeDocumentValidator()
    {
        RuleFor(x => x.NumDocument).MustBeValueObject(NumDocument.Of);
        RuleFor(x => x.CreatedAt).MustBeValueObject(CreatedAt.Of);
    }
}