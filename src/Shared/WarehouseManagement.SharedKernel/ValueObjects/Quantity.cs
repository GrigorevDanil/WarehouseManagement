using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class Quantity : ValueObject
{
    private Quantity(int value)
    {
        Value = value;
    }
    public int Value { get; }

    public static Result<Quantity, Error> Of(int value)
    {
        if (value <= 0) 
            return Errors.General.ValueIsInvalid(nameof(Quantity));

        return new Quantity(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}