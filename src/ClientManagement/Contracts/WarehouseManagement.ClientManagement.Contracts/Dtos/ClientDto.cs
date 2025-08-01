namespace WarehouseManagement.ClientManagement.Contracts.Dtos;

public class ClientDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public string Address { get; init; } = string.Empty;
    
    public bool IsArchived { get; init; }
    
    public DateTime? ArchivedAt { get; init; }
}