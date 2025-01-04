using Microsoft.EntityFrameworkCore;

namespace OpenTelemetry.Login.Database;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<TelemetryItem> TelemetryItems { get; set; }
    public DbSet<LogRecordItem> LogRecords { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
