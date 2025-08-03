using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class IncomeResourceId: ValueObject, IComparable<IncomeResourceId>
{
    private IncomeResourceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static IncomeResourceId Create() => new(Guid.NewGuid());
        
    public static IncomeResourceId Empty() => new(Guid.Empty);
        
    public static IncomeResourceId Of(Guid id) => new(id);
        
    public int CompareTo(IncomeResourceId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}