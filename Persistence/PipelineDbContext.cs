using Microsoft.EntityFrameworkCore;
using PipelineApp2._0.Domain;

namespace PipelineApp2._0.Persistence;

public class PipelineDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<DateEntry> DateEntries { get; set; }
    public PipelineDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("PipelineDb"));
    }

}