using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.Framework.Outbox;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Payload)
            .HasColumnType("jsonb")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(o => o.Type)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired();

        builder.Property(o => o.ProcessedAt)
            .HasConversion(v => v!.Value.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired(false);

        builder.HasIndex(e => new
            {
                e.CreatedAt,
                e.ProcessedAt
            })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .IncludeProperties(e => new
            {
                e.Id,
                e.Type,
                e.Payload
            })
            .HasFilter("\"ProcessedAt\" IS NULL");
    }
}