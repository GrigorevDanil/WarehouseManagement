using WarehouseManagement.IncomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.IncomeProcessing.Application.Interfaces;

public interface IIncomeProcessingReadDbContext
{
    IQueryable<IncomeDocumentDto> IncomeDocuments { get; }
    
    IQueryable<IncomeResourceDto> IncomeResources { get; }
}