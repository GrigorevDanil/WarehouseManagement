using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.Core.Models;

public record Envelope<T>
{
    public T? Result { get; }

    public ErrorList? Errors { get; }

    public DateTime TimeGenerated { get; }

    private Envelope(T? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope<T> Ok(T? result = default) =>
        new(result, null);

    public static Envelope<T> Error(ErrorList errors) =>
        new(default, errors);
}