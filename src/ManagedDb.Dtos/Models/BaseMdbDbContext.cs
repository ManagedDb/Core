using Microsoft.EntityFrameworkCore;

namespace ManagedDb.Dtos.Models;

public abstract class BaseMdbDbContext : DbContext
{
    protected abstract Type[] ModelTypes { set; get; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = this.ModelTypes ?? throw new NullReferenceException("ModelTypes is null");

        // here we create DbSet<T> properties dynamically
        foreach (var type in this.ModelTypes)
        {
            modelBuilder.Entity(type);
        }

        base.OnModelCreating(modelBuilder);
    }
}
