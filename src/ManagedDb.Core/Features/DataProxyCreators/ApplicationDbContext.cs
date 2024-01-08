using Microsoft.EntityFrameworkCore;

namespace ManagedDb.Core.Features.DataProxyCreators;

public class ApplicationDbContext : DbContext
{
    private readonly string dbPath;

    public ApplicationDbContext()
    {
        var folder = Environment.CurrentDirectory;
        var appDataFolder = Path.Combine(folder, "App_Data");
        this.dbPath = Path.Combine(appDataFolder, "mdb.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={this.dbPath}");
}
