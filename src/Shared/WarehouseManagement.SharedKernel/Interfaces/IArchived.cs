using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.SharedKernel.Interfaces;

public interface IArchived
{
    IsArchived IsArchived { get; }
    
    ArchivedAt ArchivedAt { get; }
    
    void MoveToArchive();
    
    void RestoreFromArchive();
}