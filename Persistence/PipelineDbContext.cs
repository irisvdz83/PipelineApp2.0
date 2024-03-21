using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PipelineApp2._0.Domain;

namespace PipelineApp2._0.Persistence;

public class PipelineDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<DateEntry> DateEntries { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public PipelineDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("PipelineDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var splitStringConverter = new ValueConverter<List<string>, string>(v => string.Join(",", v), v => v.Split(new[] { ',' }).ToList());
        modelBuilder.Entity<DateEntry>()
            .Property(nameof(DateEntry.Tags))
            .HasConversion(splitStringConverter);

        var intArrayValueConverter = new ValueConverter<List<int>, string>(
            i => string.Join(",", i),
            s => string.IsNullOrWhiteSpace(s) ? new List<int>() : s.Split(new[] { ',' }).Select(int.Parse).ToList());
        modelBuilder.Entity<Setting>()
            .Property(nameof(Setting.WorkingDaysPerWeek))
            .HasConversion(intArrayValueConverter);
    }

}