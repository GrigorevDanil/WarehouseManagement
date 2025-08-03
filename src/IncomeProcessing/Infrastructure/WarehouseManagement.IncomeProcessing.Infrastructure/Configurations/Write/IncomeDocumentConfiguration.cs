using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.IncomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Configurations.Write;

public class IncomeDocumentConfiguration : IEntityTypeConfiguration<IncomeDocument>
{
    public void Configure(EntityTypeBuilder<IncomeDocument> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => IncomeDocumentId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.ComplexProperty(x => x.NumDocument, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(IncomeDocument.NumDocument))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.CreatedAt, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(IncomeDocument.CreatedAt))
                    .IsRequired();
            });

        builder.HasMany(x => x.Resources)
            .WithOne()
            .HasForeignKey(x => x.IncomeDocumentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

    }
}