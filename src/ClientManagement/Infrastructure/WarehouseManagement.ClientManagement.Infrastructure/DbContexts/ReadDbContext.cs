using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseManagement.ClientManagement.Application.Interfaces;
using WarehouseManagement.ClientManagement.Contracts.Dtos;

namespace WarehouseManagement.ClientManagement.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IClientReadDbContext
{
    public IQueryable<ClientDto> Clients => Set<ClientDto>();

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
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
}