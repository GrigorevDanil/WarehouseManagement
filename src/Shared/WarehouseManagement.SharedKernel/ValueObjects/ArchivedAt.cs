using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class ArchivedAt: ValueObject
{
    private ArchivedAt(DateTime? value)
    {
        Value = value;
    }

    public DateTime? Value { get; } = null!;
    
    public static ArchivedAt Empty => new(null);

    public static Result<ArchivedAt, Error> Of(DateTime? dateTime)
    {
        if (dateTime > DateTime.UtcNow)
            return Errors.General.ValueIsInvalid(nameof(ArchivedAt));
        
        return new ArchivedAt(dateTime);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}