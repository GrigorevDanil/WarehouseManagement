using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class NumDocument: ValueObject
{
    public const int MAX_LENGTH = 20;
    
    private NumDocument(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<NumDocument, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH) 
            return Errors.General.ValueIsRequired(nameof(NumDocument));

        return new NumDocument(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}