using WarehouseManagement.ClientManagement.Contracts.Dtos;

namespace WarehouseManagement.ClientManagement.Application.Interfaces;

public interface IClientReadDbContext
{
    IQueryable<ClientDto> Clients { get; }
}