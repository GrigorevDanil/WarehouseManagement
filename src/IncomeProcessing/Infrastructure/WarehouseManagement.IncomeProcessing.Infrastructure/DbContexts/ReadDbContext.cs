using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Constants;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Contracts.Dtos;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IIncomeProcessingReadDbContext
{
    public IQueryable<IncomeDocumentDto> IncomeDocuments => Set<IncomeDocumentDto>();
    
    public IQueryable<IncomeResourceDto> IncomeResources => Set<IncomeResourceDto>();

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
        
        modelBuilder.HasDefaultSchema(Schemas.IncomeProcessing);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

}