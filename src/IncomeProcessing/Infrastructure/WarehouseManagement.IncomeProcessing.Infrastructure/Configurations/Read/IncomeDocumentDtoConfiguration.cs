using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.IncomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.Configurations.Read;

public class IncomeDocumentDtoConfiguration : IEntityTypeConfiguration<IncomeDocumentDto>
{
    public void Configure(EntityTypeBuilder<IncomeDocumentDto> builder)
    {
        builder.ToTable("IncomeDocuments");
        
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.Resources)
            .WithOne()
            .HasForeignKey(x => x.IncomeDocumentId);
    }
}