using FluentValidation;
using WarehouseManagement.Core.Validation;

namespace WarehouseManagement.OutcomeProcessing.Application.UseCases.SignDocument;

public class SignDocumentValidator : AbstractValidator<SignDocumentCommand>
{
    public SignDocumentValidator()
    {
        RuleFor(x => x.OutcomeDocumentId).MustBeValidGuid();
    }
}