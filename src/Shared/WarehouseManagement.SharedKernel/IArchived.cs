using WarehouseManagement.SharedKernel.ValueObjects;

namespace WarehouseManagement.SharedKernel;

public interface IArchived
{
    IsArchived IsArchived { get; }
    
    ArchivedAt ArchivedAt { get; }
    
    void MoveToArchive();
    
    void RestoreFromArchive();
}