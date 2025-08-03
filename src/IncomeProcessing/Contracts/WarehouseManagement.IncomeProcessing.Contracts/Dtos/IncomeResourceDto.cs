namespace WarehouseManagement.IncomeProcessing.Contracts.Dtos;

public class IncomeResourceDto
{
    public Guid Id { get; init; }
    
    public Guid IncomeDocumentId { get; init; }
    
    public Guid ResourceId { get; init; }
    
    public Guid UnitId { get; init; }
    
    public int ResourceStock { get; init; }
}