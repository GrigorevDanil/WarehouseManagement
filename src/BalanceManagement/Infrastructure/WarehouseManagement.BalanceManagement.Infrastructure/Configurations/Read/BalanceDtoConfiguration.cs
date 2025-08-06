using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.BalanceManagement.Contracts.Dtos;

namespace WarehouseManagement.BalanceManagement.Infrastructure.Configurations.Read;

public class BalanceDtoConfiguration: IEntityTypeConfiguration<BalanceDto>
{
    public void Configure(EntityTypeBuilder<BalanceDto> builder)
    {
        builder.ToTable("Balances");
        
        builder.HasKey(x => x.Id);
    }
}