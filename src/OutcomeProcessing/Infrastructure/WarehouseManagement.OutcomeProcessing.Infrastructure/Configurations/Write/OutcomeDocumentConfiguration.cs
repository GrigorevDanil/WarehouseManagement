using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Domain.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.Configurations.Write;

public class OutcomeDocumentConfiguration: IEntityTypeConfiguration<OutcomeDocument>
{
    public void Configure(EntityTypeBuilder<OutcomeDocument> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => OutcomeDocumentId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.ComplexProperty(x => x.NumDocument, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeDocument.NumDocument))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.ClientId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeDocument.ClientId))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.CreatedAt, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeDocument.CreatedAt))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.Status, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(OutcomeDocument.Status))
                    .HasMaxLength(OutcomeDocumentStatus.MAX_LENGTH)
                    .HasConversion(x => x.ToString(), x => OutcomeDocumentStatus.Of(x).Value.Value)
                    .IsRequired();
            });

        builder.HasMany(x => x.Resources)
            .WithOne()
            .HasForeignKey(x => x.OutcomeDocumentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

    }
}