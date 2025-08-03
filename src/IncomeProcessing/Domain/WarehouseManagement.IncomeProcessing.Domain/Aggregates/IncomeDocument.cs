using CSharpFunctionalExtensions;
using WarehouseManagement.IncomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Domain.Aggregates;

/// <summary> Корень-агрегат, документ поступления </summary>
public class IncomeDocument : Entity<IncomeDocumentId>
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private IncomeDocument(IncomeDocumentId id) : base(id) { }
    
    /// <summary> Конструктор клиента </summary>
    public IncomeDocument(
        NumDocument numDocument, CreatedAt createdAt): base(IncomeDocumentId.Create())
    {
        NumDocument = numDocument;
        CreatedAt = createdAt;
    }

    /// <summary> Номер документа </summary>
    public NumDocument NumDocument { get; private set; }
    
    /// <summary> Дата создания документа </summary>
    public CreatedAt CreatedAt { get; private set; }
    
    /// <summary> Поступающие ресурсы для взаимодействия внутри корня-агрегата</summary>
    private readonly List<IncomeResource> _resources = [];
    
    /// <summary> Поступающие ресурсы </summary>
    public IReadOnlyList<IncomeResource> Resources => _resources;

    /// <summary>
    /// Обновление информации о документе поступления
    /// </summary>
    /// <param name="numDocument">Номер документа</param>
    /// <param name="createdAt">Дата создания документа поступления</param>
    public void UpdateMainInfo(NumDocument numDocument, CreatedAt createdAt)
    {
        NumDocument =  numDocument;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Получить ресурс по идентификатору
    /// </summary>
    /// <param name="incomeResourceId">Идентификатор ресурса</param>
    /// <returns></returns>
    public Result<IncomeResource, Error> GetIncomeResourceById(Guid incomeResourceId)
    {
        var incomeResource = _resources.FirstOrDefault(x => x.Id.Value == incomeResourceId);
        
        if (incomeResource == null)
            return Errors.General.NotFound(incomeResourceId);

        return incomeResource;
    }

    /// <summary>
    /// Добавить ресурс к документу поступления
    /// </summary>
    /// <param name="resource">Ресурс поступления</param>
    public void AddResource(IncomeResource resource)
    {
        _resources.Add(resource);
    }
    
    /// <summary>
    /// Удалить ресурс из документа поступления
    /// </summary>
    /// <param name="resource">Ресурс поступления</param>
    public void DeleteResource(IncomeResource resource)
    {
        _resources.Remove(resource);
    }
}