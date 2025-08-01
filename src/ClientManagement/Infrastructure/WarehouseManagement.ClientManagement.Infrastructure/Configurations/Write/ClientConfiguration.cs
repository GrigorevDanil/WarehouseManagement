using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.ClientManagement.Domain.Entities;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ClientManagement.Infrastructure.Configurations.Write;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => ClientId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.ComplexProperty(x => x.Title, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Client.Title))
                    .HasMaxLength(Title.MAX_LENGTH)
                    .IsRequired();
            });
        
        builder.Property(e => e.Address)
            .HasConversion(
                v => v.ToString(),
                v => Address.Of(v).Value)
            .HasMaxLength(Address.MAX_ADDRESS_LENGTH);
        
        builder.ComplexProperty(x => x.IsArchived, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Client.IsArchived))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.ArchivedAt, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Client.ArchivedAt))
                    .IsRequired(false);
            });
    }
}