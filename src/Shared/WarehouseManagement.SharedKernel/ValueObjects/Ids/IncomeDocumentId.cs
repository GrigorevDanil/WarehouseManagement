using CSharpFunctionalExtensions;

namespace WarehouseManagement.SharedKernel.ValueObjects.Ids;

public class IncomeDocumentId: ValueObject, IComparable<IncomeDocumentId>
{
    private IncomeDocumentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get;}
        
    public static IncomeDocumentId Create() => new(Guid.NewGuid());
        
    public static IncomeDocumentId Empty() => new(Guid.Empty);
        
    public static IncomeDocumentId Of(Guid id) => new(id);
        
    public int CompareTo(IncomeDocumentId? id) => Value.CompareTo(id?.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}