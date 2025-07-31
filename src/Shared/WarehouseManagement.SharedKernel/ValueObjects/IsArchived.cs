using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class IsArchived: ValueObject
{
    public static IsArchived Active => new(false);
    
    public static IsArchived Archived => new(true);
    
    private IsArchived(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static Result<IsArchived, Error> Of(bool value)
    {
        return new IsArchived(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}