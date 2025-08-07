namespace WarehouseManagement.OutcomeProcessing.Contracts.Dtos;

public class OutcomeResourceDto
{
    public Guid Id { get; init; }
    
    public Guid OutcomeDocumentId { get; init; }
    
    public Guid ResourceId { get; init; }
    
    public Guid UnitId { get; init; }
    
    public int ResourceQuantity { get; init; }
}