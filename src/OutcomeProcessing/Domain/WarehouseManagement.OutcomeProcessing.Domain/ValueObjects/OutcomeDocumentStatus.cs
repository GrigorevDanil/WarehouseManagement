using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.OutcomeProcessing.Domain.ValueObjects;

public class OutcomeDocumentStatus: ValueObject
{
    public const int MAX_LENGTH = 8; 
    public static OutcomeDocumentStatus Signed => new(nameof(Signed));
    public static OutcomeDocumentStatus UnSigned => new(nameof(UnSigned));
    public static OutcomeDocumentStatus Revoke => new(nameof(Revoke));
    
    private static readonly OutcomeDocumentStatus[] Types =
    [
        Signed, UnSigned, Revoke
    ];

    private OutcomeDocumentStatus(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<OutcomeDocumentStatus, Error> Of(string value)
    {
        var outcomeDocumentStatus = value;

        if (Types.Any(t => t.Value == outcomeDocumentStatus) == false)
        {
            return Errors.General.ValueIsInvalid(nameof(OutcomeDocumentStatus));
        }

        return new OutcomeDocumentStatus(outcomeDocumentStatus);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}