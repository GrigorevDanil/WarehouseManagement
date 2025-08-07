using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WarehouseManagement.Framework.Hosting;

public class DbContextMigrationFilter<TDbContext> : IStartupFilter 
    where TDbContext : DbContext
{
    private readonly ILogger<DbContextMigrationFilter<TDbContext>> _logger;

    public DbContextMigrationFilter(ILogger<DbContextMigrationFilter<TDbContext>> logger)
    {
        _logger = logger;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    _logger.LogInformation("Applying migrations for {DbContext}...", typeof(TDbContext).Name);
                    
                    var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                    dbContext.Database.Migrate();
                    
                    _logger.LogInformation("Migrations for {DbContext} applied successfully", typeof(TDbContext).Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to apply migrations for {DbContext}", typeof(TDbContext).Name);
                    throw;
                }
            }
            next(app);
        };
    }
}