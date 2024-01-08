using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ManagedDb.WebApi.Models;

public class ApplicationDbContext : DbContext
{
    private readonly IOptions<ManagedDbApiOptions> options;
    public ApplicationDbContext(
        IOptions<ManagedDbApiOptions> options)
    {
        this.options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={this.options.Value.DbPath}");
}
