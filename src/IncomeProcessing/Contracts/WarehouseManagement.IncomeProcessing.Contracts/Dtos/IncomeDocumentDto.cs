namespace WarehouseManagement.IncomeProcessing.Contracts.Dtos;

public class IncomeDocumentDto
{
    public Guid Id { get; init; }

    public string NumDocument { get; init; } = string.Empty;
    
    public DateTime CreatedAt { get; init; }

    public IList<IncomeResourceDto> Resources { get; init; } = [];
}