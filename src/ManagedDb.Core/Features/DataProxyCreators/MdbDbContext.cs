using ManagedDb.Dtos.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagedDb.Core.Features.DataProxyCreators;

public class MdbDbContext : BaseMdbDbContext
{
    protected override Type[] ModelTypes { get; set; }
    private readonly string dbPath;

    public MdbDbContext(Type[] modelTypes)
    {
        var folder = Environment.CurrentDirectory;
        var appDataFolder = Path.Combine(folder, "App_Data");
        this.dbPath = Path.Combine(appDataFolder, "mdb.db");

        this.ModelTypes = modelTypes;
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={this.dbPath}");
}
