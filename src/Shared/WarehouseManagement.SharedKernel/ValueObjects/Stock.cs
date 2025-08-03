using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects;

public class Stock : ValueObject
{
    private Stock(int value)
    {
        Value = value;
    }
    public int Value { get; }

    public static Result<Stock, Error> Of(int value)
    {
        if (value < 0) 
            return Errors.General.ValueIsInvalid(nameof(Stock));

        return new Stock(value);
    }
    
    public Result<Stock, Error> Add(int addedValue)
    {
        if (addedValue <= 0) 
            return Errors.General.ValueIsInvalid(nameof(Stock));
        
        return new Stock(Value + addedValue);
    }
    
    public Result<Stock, Error> Subtract(int subtractedValue)
    {
        if (subtractedValue <= 0) 
            return Errors.General.ValueIsInvalid(nameof(Stock));
        
        return new Stock(Value - subtractedValue);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}