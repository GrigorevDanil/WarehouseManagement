namespace WarehouseManagement.UnitManagement.Contracts.Dtos;

public class UnitDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public bool IsArchived { get; init; }
    
    public DateTime? ArchivedAt { get; init; }
}