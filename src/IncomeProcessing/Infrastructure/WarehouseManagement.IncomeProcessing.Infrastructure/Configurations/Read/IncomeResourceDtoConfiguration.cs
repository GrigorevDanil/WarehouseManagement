using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.IncomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Configurations.Read;

public class IncomeResourceDtoConfiguration : IEntityTypeConfiguration<IncomeResourceDto>
{
    public void Configure(EntityTypeBuilder<IncomeResourceDto> builder)
    {
        builder.ToTable("IncomeResources");
        
        builder.HasKey(x => x.Id);
    }
}