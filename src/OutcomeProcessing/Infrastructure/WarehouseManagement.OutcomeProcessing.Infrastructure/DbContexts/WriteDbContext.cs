using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Constants;
using WarehouseManagement.Framework.Outbox;
using WarehouseManagement.OutcomeProcessing.Domain.Aggregates;
using WarehouseManagement.OutcomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.OutcomeProcessing.Infrastructure.DbContexts;

public sealed class WriteDbContext : DbContext, IOutboxDbContext
{
    private readonly string _connectionString; 
    
    public DbSet<OutcomeDocument> OutcomeDocuments => Set<OutcomeDocument>();
    public DbSet<OutcomeResource> OutcomeResources => Set<OutcomeResource>();
    
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    
    public WriteDbContext(string connectionString)
    {
        _connectionString = connectionString;
        
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);

        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        
        modelBuilder.HasDefaultSchema(Schemas.OutcomeProcessing);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}