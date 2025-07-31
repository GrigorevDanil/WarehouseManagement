using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;

namespace WarehouseManagement.ResourceManagement.Infrastructure.Configurations.Read;

public class ResourceDtoConfiguration: IEntityTypeConfiguration<ResourceDto>
{
    public void Configure(EntityTypeBuilder<ResourceDto> builder)
    {
        builder.ToTable("Resources");
        
        builder.HasKey(x => x.Id);
    }
}