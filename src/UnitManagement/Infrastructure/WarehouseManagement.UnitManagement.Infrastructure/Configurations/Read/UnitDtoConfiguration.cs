using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.UnitManagement.Contracts.Dtos;

namespace WarehouseManagement.UnitManagement.Infrastructure.Configurations.Read;

public class UnitDtoConfiguration : IEntityTypeConfiguration<UnitDto>
{
    public void Configure(EntityTypeBuilder<UnitDto> builder)
    {
        builder.ToTable("Units");
        
        builder.HasKey(x => x.Id);
    }
}