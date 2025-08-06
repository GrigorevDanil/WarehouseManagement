namespace WarehouseManagement.BalanceManagement.Contracts.Dtos;

public class BalanceDto
{
    public Guid Id { get; init; }
    
    public Guid ResourceId { get; init; }
    
    public Guid UnitId { get; init; }
    
    public int ResourceStock { get; init; }
}