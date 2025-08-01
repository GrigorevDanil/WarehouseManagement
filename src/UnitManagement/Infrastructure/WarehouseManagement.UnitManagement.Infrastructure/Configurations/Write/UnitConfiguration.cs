using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;
using WarehouseManagement.UnitManagement.Domain.Entities;

namespace WarehouseManagement.UnitManagement.Infrastructure.Configurations.Write;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => UnitId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.ComplexProperty(x => x.Title, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Unit.Title))
                    .HasMaxLength(Title.MAX_LENGTH)
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.IsArchived, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Unit.IsArchived))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.ArchivedAt, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Unit.ArchivedAt))
                    .IsRequired(false);
            });
    }
}