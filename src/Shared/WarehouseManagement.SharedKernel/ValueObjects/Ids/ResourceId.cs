using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class ResourceId: ValueObject, IComparable<ResourceId>
{
    private ResourceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static ResourceId Create() => new(Guid.NewGuid());
        
    public static ResourceId Empty() => new(Guid.Empty);
        
    public static ResourceId Of(Guid id) => new(id);
        
    public int CompareTo(ResourceId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}