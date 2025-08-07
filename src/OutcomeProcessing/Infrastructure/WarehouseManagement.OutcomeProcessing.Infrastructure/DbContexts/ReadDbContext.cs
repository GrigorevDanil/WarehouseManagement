using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Constants;
using WarehouseManagement.OutcomeProcessing.Application.Interfaces;
using WarehouseManagement.OutcomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IOutcomeProcessingReadDbContext
{
    public IQueryable<OutcomeDocumentDto> OutcomeDocuments => Set<OutcomeDocumentDto>();
    
    public IQueryable<OutcomeResourceDto> OutcomeResources => Set<OutcomeResourceDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
        
        modelBuilder.HasDefaultSchema(Schemas.OutcomeProcessing);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

}