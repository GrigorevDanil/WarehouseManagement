using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WarehouseManagement.Core.Constants;
using WarehouseManagement.ResourceManagement.Application.Interfaces;
using WarehouseManagement.ResourceManagement.Contracts.Dtos;

namespace WarehouseManagement.ResourceManagement.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IResourceReadDbContext
{
    public IQueryable<ResourceDto> Resources => Set<ResourceDto>();

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
        
        modelBuilder.HasDefaultSchema(Schemas.ResourceManagement);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}