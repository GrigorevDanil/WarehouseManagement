using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class UnitId: ValueObject, IComparable<UnitId>
{
    private UnitId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static UnitId Create() => new(Guid.NewGuid());
        
    public static UnitId Empty() => new(Guid.Empty);
        
    public static UnitId Of(Guid id) => new(id);
        
    public int CompareTo(UnitId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}