namespace WarehouseManagement.ResourceManagement.Contracts.Dtos;

public class ResourceDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public bool IsArchived { get; init; }
    
    public DateTime? ArchivedAt { get; init; }
}