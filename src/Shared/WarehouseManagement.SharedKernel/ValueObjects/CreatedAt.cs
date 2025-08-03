using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class CreatedAt: ValueObject
{
    private CreatedAt(DateTime value)
    {
        Value = value;
    }

    public DateTime Value { get; }
    
    public static CreatedAt Create => new(DateTime.UtcNow);

    public static Result<CreatedAt, Error> Of(DateTime? dateTime)
    {
        if (!dateTime.HasValue) return Create;

        var dateTimeValue = dateTime.Value.ToUniversalTime();
        
        if (dateTimeValue < DateTime.UtcNow)
            return Errors.General.ValueIsInvalid(nameof(CreatedAt));

        return new CreatedAt(dateTimeValue);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}