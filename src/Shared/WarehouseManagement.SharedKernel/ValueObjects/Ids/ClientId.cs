using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class ClientId: ValueObject, IComparable<ClientId>
{
    private ClientId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static ClientId Create() => new(Guid.NewGuid());
        
    public static ClientId Empty() => new(Guid.Empty);
        
    public static ClientId Of(Guid id) => new(id);
        
    public int CompareTo(ClientId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}