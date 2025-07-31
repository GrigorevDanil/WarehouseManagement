using System.Collections;

namespace WarehouseManagement.SharedKernel;

public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public ErrorList(IEnumerable<Error> errors)
    {
        _errors = [..errors];
    }

    public void AddError(Error error) 
        =>_errors.Add(error);
    
    public bool IsEmpty() => 
        _errors.Count == 0;


    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public static implicit operator ErrorList(List<Error> errors)
        => new(errors);

    public static implicit operator ErrorList(Error error)
        => new([error]);
}