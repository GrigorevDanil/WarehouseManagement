using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.ClientManagement.Contracts.Dtos;

namespace WarehouseManagement.ClientManagement.Infrastructure.Configurations.Read;

public class ClientDtoConfiguration: IEntityTypeConfiguration<ClientDto>
{
    public void Configure(EntityTypeBuilder<ClientDto> builder)
    {
        builder.ToTable("Clients");
        
        builder.HasKey(x => x.Id);
    }
}