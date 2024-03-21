using PipelineApp2._0.Domain;

namespace PipelineApp2._0.ViewModels;

public class DateEntryViewModel
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Tags { get; set; }
    public string? Description { get; set; }

    public static DateEntryViewModel MapToDateEntry(DateEntry date)
    {
        return new DateEntryViewModel
        {
            Id = date.Id,
            StartTime = date.StartTime,
            EndTime = date.EndTime,
            Tags = date.Tags.Count > 0 ? string.Join(",", date.Tags) : string.Empty,
            Description = date.Description
        };
    }
}