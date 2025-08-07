using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class OutcomeDocumentId: ValueObject, IComparable<OutcomeDocumentId>
{
    private OutcomeDocumentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static OutcomeDocumentId Create() => new(Guid.NewGuid());
        
    public static OutcomeDocumentId Empty() => new(Guid.Empty);
        
    public static OutcomeDocumentId Of(Guid id) => new(id);
        
    public int CompareTo(OutcomeDocumentId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}