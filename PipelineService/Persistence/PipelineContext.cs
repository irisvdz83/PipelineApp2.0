using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PipelineService.Domain;

namespace PipelineService.Persistence;

public class PipelineContext : DbContext
{
    public DbSet<DateEntry> DateEntries => Set<DateEntry>();
    public PipelineContext(){}
    public PipelineContext(DbContextOptions options) : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PipelineContext).Assembly); // mappings from Lansweeper.Discovery.Hub.Persistence
        
        if (!string.Equals(Database.ProviderName, "Microsoft.EntityFrameworkCore.Sqlite", StringComparison.Ordinal)) return;
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties();

            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToUtcDateTimeTicksConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, and always stores in UTC, but should be sufficient for most use cases.
                
            foreach (var property in properties.Where(p => p.PropertyType == typeof(TimeSpan) || p.PropertyType == typeof(TimeSpan?)))
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(new TimeSpanToTicksConverter()); // The converter!
        }
    }
}


