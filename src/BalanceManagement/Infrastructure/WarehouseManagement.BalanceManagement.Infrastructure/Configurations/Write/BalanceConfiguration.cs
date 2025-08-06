using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.BalanceManagement.Domain.Entities;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.BalanceManagement.Infrastructure.Configurations.Write;

public class BalanceConfiguration: IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, idGuid => BalanceId.Of(idGuid))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.ComplexProperty(x => x.ResourceId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Balance.ResourceId))
                    .IsRequired();
            });
        
        builder.ComplexProperty(x => x.UnitId, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Balance.UnitId))
                    .IsRequired();

            });
        
        builder.ComplexProperty(x => x.ResourceStock, 
            p =>
            {
                p.Property(x => x.Value)
                    .HasColumnName(nameof(Balance.ResourceStock))
                    .IsRequired();
            });
    }
}