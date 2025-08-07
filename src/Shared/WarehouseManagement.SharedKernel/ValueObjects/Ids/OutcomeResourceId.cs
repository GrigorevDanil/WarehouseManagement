using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class OutcomeResourceId: ValueObject, IComparable<OutcomeResourceId>
{
    private OutcomeResourceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static OutcomeResourceId Create() => new(Guid.NewGuid());
        
    public static OutcomeResourceId Empty() => new(Guid.Empty);
        
    public static OutcomeResourceId Of(Guid id) => new(id);
        
    public int CompareTo(OutcomeResourceId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}