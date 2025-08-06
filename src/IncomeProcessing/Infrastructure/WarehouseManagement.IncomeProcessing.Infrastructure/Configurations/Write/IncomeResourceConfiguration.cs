using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.IncomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Configurations.Write;

public class IncomeResourceConfiguration : IEntityTypeConfiguration<IncomeResource>
{
    public void Configure(EntityTypeBuilder<IncomeResource> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => IncomeResourceId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedNever();
        
        builder.ComplexProperty(x => x.ResourceId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(IncomeResource.ResourceId))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.UnitId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(IncomeResource.UnitId))
                    .IsRequired();

            });
        
        builder.ComplexProperty(x => x.ResourceStock, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(IncomeResource.ResourceStock))
                    .IsRequired();
            });
    }
}