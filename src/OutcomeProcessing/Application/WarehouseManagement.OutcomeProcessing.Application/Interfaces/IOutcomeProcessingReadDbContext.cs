using WarehouseManagement.OutcomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.OutcomeProcessing.Application.Interfaces;

public interface IOutcomeProcessingReadDbContext
{
    IQueryable<OutcomeDocumentDto> OutcomeDocuments { get; }
    
    IQueryable<OutcomeResourceDto> OutcomeResources { get; }
}