using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.OutcomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.Configurations.Read;

public class OutcomeResourceDtoConfiguration: IEntityTypeConfiguration<OutcomeResourceDto>
{
    public void Configure(EntityTypeBuilder<OutcomeResourceDto> builder)
    {
        builder.ToTable("OutcomeResources");
        
        builder.HasKey(x => x.Id);
    }
}