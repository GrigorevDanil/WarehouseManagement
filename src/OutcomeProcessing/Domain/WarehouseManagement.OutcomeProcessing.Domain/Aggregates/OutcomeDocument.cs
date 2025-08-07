using CSharpFunctionalExtensions;
using WarehouseManagement.OutcomeProcessing.Domain.Entities;
using WarehouseManagement.OutcomeProcessing.Domain.ValueObjects;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Domain.Aggregates;

/// <summary> Сущность-агрегат, документ отгрузки </summary>
public class OutcomeDocument: Entity<OutcomeDocumentId>
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private OutcomeDocument(OutcomeDocumentId id) : base(id) { }
    
    /// <summary> Конструктор документа отгрузки </summary>
    public OutcomeDocument(
        NumDocument numDocument, 
        ClientId clientId, 
        CreatedAt createdAt): base(OutcomeDocumentId.Create())
    {
        NumDocument = numDocument;
        ClientId = clientId;
        CreatedAt = createdAt;
        Status = OutcomeDocumentStatus.UnSigned;
    }

    /// <summary> Номер документа </summary>
    public NumDocument NumDocument { get; private set; }
    
    /// <summary> Идентификатор клиента </summary>
    public ClientId ClientId { get; private set; }
    
    /// <summary> Дата создания документа </summary>
    public CreatedAt CreatedAt { get; private set; }
    
    /// <summary> Состояние документа отгрузки </summary>
    public OutcomeDocumentStatus Status { get; private set; }
    
    /// <summary> Ресурсы отгрузки для взаимодействия внутри сущности-агрегата</summary>
    private readonly List<OutcomeResource> _resources = [];
    
    /// <summary> Ресурсы отгрузки </summary>
    public IReadOnlyList<OutcomeResource> Resources => _resources;

    /// <summary>
    /// Обновление информации о документе отгрузки
    /// </summary>
    /// <param name="numDocument">Номер документа</param>
    /// <param name="createdAt">Дата создания документа поступления</param>
    /// <param name="clientId">Идентификатор клиента</param>
    public void UpdateMainInfo(
        NumDocument numDocument,
        ClientId clientId,
        CreatedAt createdAt
        )
    {
        NumDocument =  numDocument;
        ClientId = clientId;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Подписать документ
    /// </summary>
    public UnitResult<Error> SignDocument()
    {
        if (Status == OutcomeDocumentStatus.Signed) 
            return Errors.OutcomeDocument.DocumentAlreadySigned();
        
        Status = OutcomeDocumentStatus.Signed;

        return UnitResult.Success<Error>();
    } 
    
    /// <summary>
    /// Отозвать документ
    /// </summary>
    public UnitResult<Error> RevokeDocument()
    {
        if (Status == OutcomeDocumentStatus.UnSigned) 
            return Errors.OutcomeDocument.DocumentNotSigned();
        
        Status = OutcomeDocumentStatus.Revoke;

        return UnitResult.Success<Error>();
    } 

    /// <summary>
    /// Получить ресурс по идентификатору
    /// </summary>
    /// <param name="incomeResourceId">Идентификатор ресурса</param>
    /// <returns></returns>
    public Result<OutcomeResource, Error> GetOutcomeResourceById(Guid incomeResourceId)
    {
        var outcomeResource = _resources.FirstOrDefault(x => x.Id.Value == incomeResourceId);
        
        if (outcomeResource == null)
            return Errors.General.NotFound(incomeResourceId);

        return outcomeResource;
    }

    /// <summary>
    /// Добавить ресурс к документу отгрузки
    /// </summary>
    /// <param name="resource">Ресурс отгрузки</param>
    public void AddResource(OutcomeResource resource)
    {
        _resources.Add(resource);
    }
    
    /// <summary>
    /// Удалить ресурс из документа отгрузки
    /// </summary>
    /// <param name="resource">Ресурс отгрузки</param>
    public void DeleteResource(OutcomeResource resource)
    {
        _resources.Remove(resource);
    }
}