using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.OutcomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.Configurations.Write;

public class OutcomeResourceConfiguration: IEntityTypeConfiguration<OutcomeResource>
{
    public void Configure(EntityTypeBuilder<OutcomeResource> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => OutcomeResourceId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedNever();
        
        builder.ComplexProperty(x => x.ResourceId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeResource.ResourceId))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.UnitId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeResource.UnitId))
                    .IsRequired();

            });
        
        builder.ComplexProperty(x => x.ResourceQuantity, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeResource.ResourceQuantity))
                    .IsRequired();
            });
    }
}