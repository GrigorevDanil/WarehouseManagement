using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Abstractions.Outbox;
using WarehouseManagement.Core.Constants;
using WarehouseManagement.Core.Models;
using WarehouseManagement.Framework.Outbox;
using WarehouseManagement.IncomeProcessing.Application.Interfaces;
using WarehouseManagement.IncomeProcessing.Domain.Aggregates;
using WarehouseManagement.IncomeProcessing.Domain.Entities;
using WarehouseManagement.SharedKernel;

namespace WarehouseManagement.IncomeProcessing.Infrastructure.DbContexts;

public class WriteDbContext(string connectionString) : DbContext, IOutboxDbContext
{
    public DbSet<IncomeDocument> IncomeDocuments => Set<IncomeDocument>();
    
    public DbSet<IncomeResource> IncomeResources => Set<IncomeResource>();
    
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);

        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        
        modelBuilder.HasDefaultSchema(Schemas.IncomeProcessing);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}