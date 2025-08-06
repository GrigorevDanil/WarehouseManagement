namespace WarehouseManagement.SharedKernel;

public class OutboxMessage
{
    public Guid Id { get; init; }

    public required string Type { get; init; } 

    public required string Payload { get; init; } 
    
    public required DateTime CreatedAt { get; init; }
    
    public DateTime? ProcessedAt { get; set; }
    
    public string? Error { get; set; }
}