using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.ResourceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Infrastructure.Configurations.Write;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => ResourceId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.ComplexProperty(x => x.Title, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Resource.Title))
                    .HasMaxLength(Title.MAX_LENGTH)
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.IsArchived, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Resource.IsArchived))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.ArchivedAt, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Resource.ArchivedAt))
                    .IsRequired(false);
            });
        
    }
}