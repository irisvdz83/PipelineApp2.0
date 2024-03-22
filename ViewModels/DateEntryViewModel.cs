using PipelineApp2._0.Domain;

namespace PipelineApp2._0.ViewModels;

public class DateEntryViewModel
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Tags { get; set; }
    public string? Description { get; set; }
    public TimeSpan? TotalTime { get; set; }

    public static DateEntryViewModel MapToDateEntry(DateEntry date)
    {
        return new DateEntryViewModel
        {
            Id = date.Id,
            StartTime = date.StartTime,
            EndTime = date.EndTime,
            Tags = date.Tags.Count > 0 ? string.Join(",", date.Tags) : string.Empty,
            Description = date.Description,
            TotalTime = date.EndTime.HasValue ? date.EndTime - date.StartTime : null
        };
    }

    public List<string> GetTags()
    {
        var separateTags = Tags?.Split(',');
        
        return separateTags?.ToList() ?? new List<string>();
    }
}