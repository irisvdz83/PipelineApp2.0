using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PipelineService.Domain;

namespace PipelineService.Persistence.Mappings;

public class DateEntryMapping : IEntityTypeConfiguration<DateEntry>
{
    public void Configure(EntityTypeBuilder<DateEntry> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("TEXT").IsRequired();
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired(false);
    }
}
