namespace WarehouseManagement.OutcomeProcessing.Contracts.Dtos;

public class OutcomeDocumentDto
{
    public Guid Id { get; init; }
    
    public string NumDocument { get; init; } = string.Empty;
    
    public Guid ClientId { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public string Status { get; init; } = string.Empty;
    
    public IList<OutcomeResourceDto> Resources { get; init; } = [];
}