using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.OutcomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.Configurations.Read;

public class OutcomeDocumentDtoConfiguration: IEntityTypeConfiguration<OutcomeDocumentDto>
{
    public void Configure(EntityTypeBuilder<OutcomeDocumentDto> builder)
    {
        builder.ToTable("OutcomeDocuments");
        
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.Resources)
            .WithOne()
            .HasForeignKey(x => x.OutcomeDocumentId);
    }
}