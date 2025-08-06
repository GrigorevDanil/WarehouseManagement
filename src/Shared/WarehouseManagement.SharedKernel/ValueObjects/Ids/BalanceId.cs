using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class BalanceId: ValueObject, IComparable<BalanceId>
{
    private BalanceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static BalanceId Create() => new(Guid.NewGuid());
        
    public static BalanceId Empty() => new(Guid.Empty);
        
    public static BalanceId Of(Guid id) => new(id);
        
    public int CompareTo(BalanceId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}